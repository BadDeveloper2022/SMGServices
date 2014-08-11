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

        private event SendEventHandler onSend;
        private event RecvEventHandler onRecv;

        public event SendEventHandler OnSend
        {
            add { onSend += value; }
            remove { onSend -= value; }
        }

        public event RecvEventHandler OnRecv
        {
            add { onRecv += value; }
            remove { onRecv -= value; }
        }

        #endregion

        private string ip;
        private int port;
        private byte[] buffer;
        //private List<byte> recvbuffers;
        private bool connected;
        private Socket workSocket;
        private Thread recvThread;
        private ManualResetEvent recvDone;
        private ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        public string LocalIPAddress { get; private set; }

        public TcpSocketClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            //this.recvbuffers = new List<byte>();
            this.buffer = new byte[TransferSet.BufferSize];
            recvDone = new ManualResetEvent(false);
        }

        public TcpSocketClient(Socket socket)
        {
            this.workSocket = socket;
            //this.recvbuffers = new List<byte>();
            this.buffer = new byte[TransferSet.BufferSize];
            recvDone = new ManualResetEvent(false);
            this.LocalIPAddress = socket.RemoteEndPoint.ToString();
            connected = true;
        }

        public void Connect()
        {
            try
            {
                if (!connected)
                {
                    workSocket = workSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    
                    workSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), (ar) =>
                    {
                        try
                        {
                            workSocket.EndConnect(ar);
                            connected = true;
                            this.LocalIPAddress = workSocket.LocalEndPoint.ToString();
                        }
                        catch (SocketException e)
                        {
                            SocketExceptionHandler(e);
                        }
                    }, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                int len = workSocket.EndReceive(ar, out sokcetError);
                //每个消息长度不超过2K
                if (len > 0)
                {
                    byte[] data = new byte[len];
                    Buffer.BlockCopy(buffer, 0, data, 0, len);
                    buffer = new byte[TransferSet.BufferSize];

                    if (onRecv != null)
                    {
                        //执行所有接受委托事件
                        foreach (var inv in onRecv.GetInvocationList())
                        {
                            try
                            {
                                var _onRecv = (RecvEventHandler)inv;
                                _onRecv(this, data);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Invoke Delegate OnRecv Catch ：" + e);
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
            if (connected)
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
                            while (connected)
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
                    Console.WriteLine(e);
                }
            }
        }

        public void Send(byte[] data)
        {
            try
            {
                if (connected)
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
                                        var _onSend = (RecvEventHandler)inv;
                                        _onSend(this, data);
                                    }
                                    catch (Exception e)
                                    {
                                        Console.WriteLine("Invoke Delegate OnSend Catch ：" + e);
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
                if (connected)
                {
                    if (recvThread != null && recvThread.IsAlive)
                    {
                        recvThread.Abort();
                    }

                    workSocket.BeginDisconnect(false, (ar) =>
                    {
                        try
                        {
                            workSocket.EndDisconnect(ar);
                            workSocket.Shutdown(SocketShutdown.Both);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            connected = false;

                            try
                            {
                                workSocket.Close();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                            Console.WriteLine(this.LocalIPAddress + " 已断开连接");
                        }
                    }, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                Console.WriteLine(e);
            }
        }

    }
}
