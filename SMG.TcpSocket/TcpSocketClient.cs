using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;

namespace SMG.TcpSocket
{
    public class TcpSocketClient
    {
        #region events

        private event ConnectedEventHandler onConnected;
        private event DisconnectedEventHandler onDisconnected;
        private event SendEventHandler onSend;
        private event ReadEventHandler onRead;
        private event ExceptionEventHandler onException;

        public event ConnectedEventHandler OnConnected
        {
            add { onConnected += value; }
            remove { onConnected -= value; }
        }

        public event DisconnectedEventHandler OnDisconnected
        {
            add { onDisconnected += value; }
            remove { onDisconnected -= value; }
        }

        public event SendEventHandler OnSend
        {
            add { onSend += value; }
            remove { onSend -= value; }
        }

        public event ReadEventHandler OnRead
        {
            add { onRead += value; }
            remove { onRead -= value; }
        }

        public event ExceptionEventHandler OnException
        {
            add { onException += value; }
            remove { onException -= value; }
        }

        #endregion

        #region request events

        private void RequestConnectedEvent()
        {
            if (onConnected != null)
            {
                foreach (var inv in onConnected.GetInvocationList())
                {
                    var invoke = (ConnectedEventHandler)inv;
                    invoke(this);
                }
            }
        }

        private void RequestDisconnectedEvent()
        {
            if (onDisconnected != null)
            {
                foreach (var inv in onDisconnected.GetInvocationList())
                {
                    var invoke = (DisconnectedEventHandler)inv;
                    invoke(this);
                }
            }
        }

        private void RequestSendEvent(byte[] buffer)
        {
            if (onSend != null)
            {
                foreach (var inv in onSend.GetInvocationList())
                {
                    var invoke = (SendEventHandler)inv;
                    invoke(this, buffer);
                }
            }
        }

        private void RequestReadEvent(byte[] buffer)
        {
            if (onRead != null)
            {
                foreach (var inv in onRead.GetInvocationList())
                {
                    var invoke = (ReadEventHandler)inv;
                    invoke(this, buffer);
                }
            }
        }

        private void RequestExceptionEvent(Exception e)
        {
            bool progress = false;

            if (e is SocketException)
            {
                var ex = e as SocketException;

                switch (ex.SocketErrorCode)
                {
                    case SocketError.ConnectionAborted:
                    case SocketError.ConnectionReset:
                        Disconnect();
                        progress = true;
                        break;
                    default:
                        break;
                }
            }

            if (!progress && onException != null)
            {
                foreach (var inv in onException.GetInvocationList())
                {
                    var invoke = (ExceptionEventHandler)inv;
                    invoke(e);
                }
            }
        }

        #endregion

        private string ip;
        private int port;
        private byte[] buffer;
        private Socket workSocket;
        private Thread recvThread;
        private ManualResetEvent recvDone;
        private System.Timers.Timer sendTimer;
        private Queue<byte[]> sendQueue;
        private ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        public string LocalIPAddress { get; private set; }

        public string RemoteIPAddress { get; private set; }

        public bool Connected { get; private set; }

        public TcpSocketClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            this.buffer = new byte[TransferSet.BufferSize];
            recvDone = new ManualResetEvent(false);
            sendQueue = new Queue<byte[]>();
        }

        public TcpSocketClient(Socket socket)
        {
            this.workSocket = socket;
            this.buffer = new byte[TransferSet.BufferSize];
            recvDone = new ManualResetEvent(false);
            sendQueue = new Queue<byte[]>();
            this.LocalIPAddress = socket.RemoteEndPoint.ToString();
            this.RemoteIPAddress = socket.LocalEndPoint.ToString();
            Connected = true;
        }

        public void Connect()
        {
            try
            {
                if (!Connected)
                {
                    workSocket = workSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    workSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), (ar) =>
                    {
                        try
                        {
                            workSocket.EndConnect(ar);
                            Connected = true;
                            this.LocalIPAddress = workSocket.LocalEndPoint.ToString();
                            this.RemoteIPAddress = workSocket.RemoteEndPoint.ToString();
                            //请求委托事件
                            this.RequestConnectedEvent();
                        }
                        catch (Exception e)
                        {
                            RequestExceptionEvent(e);
                        }
                    }, null);
                }
            }
            catch (Exception e)
            {
                onException(e);
            }
        }

        private int readZeroCount = 0;

        private void ResetZeroRecvCount()
        {
            locker.EnterWriteLock();
            readZeroCount = 0;
            locker.ExitWriteLock();
        }

        /// <summary>
        /// 0字节读取处理（10次读取）前5次累加，后5次每次等待1秒
        /// </summary>
        private void ZeroRecvHandle()
        {
            if (readZeroCount == 10)
            {
                Disconnect();
                readZeroCount = 0;
            }
            else
            {
                if (readZeroCount >= 5)
                {
                    Thread.Sleep(1000);
                }
                readZeroCount++;
            }
        }

        private void RecvCallback(IAsyncResult ar)
        {
            SocketError sokcetError = SocketError.SocketError;

            try
            {
                if (Connected)
                {
                    int len = workSocket.EndReceive(ar, out sokcetError);
                    //每个消息长度不超过2K
                    if (len > 0)
                    {
                        byte[] data = new byte[len];
                        Buffer.BlockCopy(buffer, 0, data, 0, len);
                        //请求委托事件
                        RequestReadEvent(data);
                        buffer = new byte[TransferSet.BufferSize];
                        //重置0字节读取次数
                        ResetZeroRecvCount();
                    }
                    else
                    {
                        ZeroRecvHandle();
                    }
                }

                recvDone.Set();
            }
            catch (Exception e)
            {
                RequestExceptionEvent(e);
                recvDone.Set();
            }
        }

        private void BeginSend(byte[] data)
        {
            if (Connected)
            {
                try
                {
                    workSocket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
                    {
                        try
                        {
                            SocketError socketError = SocketError.SocketError;
                            int len = workSocket.EndSend(ar, out socketError);
                            if (len < data.Length) throw new SocketException((int)socketError);

                            //请求委托事件
                            RequestSendEvent(data);
                        }
                        catch (Exception ex)
                        {
                            RequestExceptionEvent(ex);
                        }
                    }, null);
                }
                catch (Exception ex)
                {
                    RequestExceptionEvent(ex);
                }
            }
        }

        public void Start()
        {
            if (Connected)
            {
                try
                {
                    if (recvThread != null && recvThread.IsAlive)
                    {
                        recvThread.Abort();
                    }

                    recvThread = new Thread(() =>
                    {
                        try
                        {
                            while (Connected)
                            {
                                recvDone.Reset();
                                workSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(RecvCallback), null);
                                recvDone.WaitOne();
                            }
                        }
                        catch (Exception e)
                        {
                            RequestExceptionEvent(e);
                        }
                    });
                    recvThread.IsBackground = true;
                    recvThread.Start();

                    if (sendTimer != null)
                    {
                        sendTimer.Stop();
                    }
                    //每100毫秒发送一条消息，暂时用于解决粘包问题
                    sendTimer = new System.Timers.Timer(100);
                    sendTimer.Elapsed += (sender, e) =>
                    {
                        if (Connected)
                        {
                            locker.EnterWriteLock();

                            if (sendQueue.Count > 0)
                            {
                                var data = sendQueue.Dequeue();
                                BeginSend(data);
                            }

                            locker.ExitWriteLock();
                        }
                    };
                    sendTimer.Start();
                }
                catch (Exception e)
                {
                    RequestExceptionEvent(e);
                }
            }
        }

        public void Send(byte[] data)
        {
            if (Connected)
            {
                locker.EnterWriteLock();
                sendQueue.Enqueue(data);
                locker.ExitWriteLock();
            }
        }

        public void Disconnect()
        {
            try
            {
                if (Connected)
                {
                    workSocket.BeginDisconnect(false, (ar) =>
                    {
                        locker.EnterWriteLock();

                        if (Connected)
                        {
                            //请求委托事件
                            RequestDisconnectedEvent();

                            try
                            {
                                sendTimer.Stop();

                                workSocket.EndDisconnect(ar);
                                workSocket.Shutdown(SocketShutdown.Both);
                            }
                            catch (Exception e)
                            {
                                RequestExceptionEvent(e);
                            }
                            finally
                            {
                                Connected = false;

                                try
                                {
                                    workSocket.Close();
                                }
                                catch (Exception e)
                                {
                                    RequestExceptionEvent(e);
                                }
                            }
                        }

                        locker.ExitWriteLock();

                    }, null);
                }
            }
            catch (Exception e)
            {
                RequestExceptionEvent(e);
            }
        }
    }
}
