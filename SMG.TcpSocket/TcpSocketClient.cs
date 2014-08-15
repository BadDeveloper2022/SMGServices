using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

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

        private string ip;
        private int port;
        private byte[] buffer;
        private Socket workSocket;
        private Thread recvThread;
        private ManualResetEvent recvDone;
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
        }

        public TcpSocketClient(Socket socket)
        {
            this.workSocket = socket;
            this.buffer = new byte[TransferSet.BufferSize];
            recvDone = new ManualResetEvent(false);
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

                            if (onConnected != null)
                            {
                                //执行所有接受委托事件
                                foreach (var inv in onConnected.GetInvocationList())
                                {
                                    try
                                    {
                                        var _onConnected = (ConnectedEventHandler)inv;
                                        _onConnected(this);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Invoke Delegate OnConnected Catch Exception \n" + e);
                                    }
                                }
                            }
                        }
                        catch (SocketException e)
                        {
                            SocketExceptionHandler(e);
                        }
                        catch (ObjectDisposedException e)
                        {
                            onException(e);
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
                        buffer = new byte[TransferSet.BufferSize];

                        if (onRead != null)
                        {
                            //执行所有接受委托事件
                            foreach (var inv in onRead.GetInvocationList())
                            {
                                try
                                {
                                    var _onRead = (ReadEventHandler)inv;
                                    _onRead(this, data);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Invoke Delegate OnRead Catch Exception \n" + e);
                                }
                            }
                        }

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
            catch (SocketException e)
            {
                SocketExceptionHandler(e);
                recvDone.Set();
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
                        catch (SocketException e)
                        {
                            SocketExceptionHandler(e);
                        }
                    });
                    recvThread.IsBackground = true;
                    recvThread.Start();
                }
                catch (Exception e)
                {
                    onException(e);
                }
            }
        }

        public void Send(byte[] data)
        {
            try
            {
                if (Connected)
                {
                    workSocket.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
                    {
                        try
                        {
                            SocketError socketError = SocketError.SocketError;
                            int len = workSocket.EndSend(ar, out socketError);
                            if (len < data.Length) throw new SocketException((int)socketError);

                            if (onSend != null)
                            {
                                //执行所有接受委托事件
                                foreach (var inv in onSend.GetInvocationList())
                                {
                                    try
                                    {
                                        var _onSend = (SendEventHandler)inv;
                                        _onSend(this, data);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Invoke Delegate OnSend Catch Exception \n" + e);
                                    }
                                }
                            }
                        }
                        catch (SocketException e)
                        {
                            SocketExceptionHandler(e);
                        }
                    }, null);
                }
            }
            catch (SocketException e)
            {
                SocketExceptionHandler(e);
            }
        }

        public void Disconnect()
        {
            locker.EnterWriteLock();

            try
            {
                if (Connected)
                {
                    workSocket.BeginDisconnect(false, (ar) =>
                    {
                        if (onDisconnected != null)
                        {
                            //执行所有接受委托事件
                            foreach (var inv in onDisconnected.GetInvocationList())
                            {
                                try
                                {
                                    var _onDisconnected = (DisconnectedEventHandler)inv;
                                    _onDisconnected(this);
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Invoke Delegate OnDisconnected Catch Exception \n" + e);
                                }
                            }
                        }

                        try
                        {
                            workSocket.EndDisconnect(ar);
                            workSocket.Shutdown(SocketShutdown.Both);
                        }
                        catch (Exception e)
                        {
                            onException(e);
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
                                onException(e);
                            }
                        }
                    }, null);
                }
            }
            catch (Exception e)
            {
                onException(e);
            }

            locker.ExitWriteLock();
        }

        private void SocketExceptionHandler(SocketException e)
        {
            if (e.SocketErrorCode == SocketError.ConnectionAborted ||
                e.SocketErrorCode == SocketError.ConnectionReset)
            {
                this.Disconnect();
            }
            else
            {
                onException(e);
            }
        }

    }
}
