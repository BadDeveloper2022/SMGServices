using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SMG.TcpSocket
{
    public class TcpSocketServer
    {
        #region events

        private event SendEventHandler onSend;
        private event ReadEventHandler onRead;
        private event StartEventHandler onStart;
        private event StopEventHandler onStop;
        private event ConnectedEventHandler onConnected;
        private event DisconnectedEventHandler onDisconnected;

        public event StartEventHandler OnStart
        {
            add { onStart += value; }
            remove { onStart -= value; }
        }

        public event StopEventHandler OnStop
        {
            add { onStop += value; }
            remove { onStop -= value; }
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

        #endregion

        private string ip;
        private int port;
        private Socket workSocket;
        private Thread acceptThread;
        private ManualResetEvent accpetDone;
        private ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        public int BackLog { get; private set; }

        public bool Listened { get; private set; }

        public string BindIPAddress
        {
            get
            {
                if (workSocket != null && Listened)
                {
                    return workSocket.LocalEndPoint.ToString();
                }

                return "";
            }
        }

        public TcpSocketServer(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            this.accpetDone = new ManualResetEvent(false);
        }

        public void Listen(int poolSize)
        {
            try
            {
                if (!Listened)
                {
                    workSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    workSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
                    workSocket.Listen(poolSize);
                    Listened = true;
                    this.BackLog = poolSize;

                    if (acceptThread != null && acceptThread.IsAlive)
                    {
                        acceptThread.Abort();
                    }

                    acceptThread = new Thread(() =>
                    {
                        while (Listened)
                        {
                            accpetDone.Reset();

                            workSocket.BeginAccept((ar) =>
                            {
                                try
                                {
                                    if (Listened)
                                    {
                                        var client = workSocket.EndAccept(ar);
                                        var tcpClient = new TcpSocketClient(client);
                                        tcpClient.OnRead += onRead;
                                        tcpClient.OnSend += onSend;
                                        tcpClient.OnDisconnected += onDisconnected;
                                        tcpClient.Start();

                                        if (onConnected != null)
                                        {
                                            //执行所有接受委托事件
                                            foreach (var inv in onConnected.GetInvocationList())
                                            {
                                                try
                                                {
                                                    var _onConnected = (ConnectedEventHandler)inv;
                                                    _onConnected(tcpClient);
                                                }
                                                catch (Exception e)
                                                {
                                                    Console.WriteLine("Invoke Delegate OnConnected Catch Exception \n" + e);
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (SocketException e)
                                {
                                    Console.WriteLine(e);
                                }
                                finally
                                {
                                    accpetDone.Set();
                                }
                            }, null);

                            accpetDone.WaitOne();
                        }
                    });
                    acceptThread.IsBackground = true;
                    acceptThread.Start();

                    if (onStart != null)
                    {
                        //执行所有接受委托事件
                        foreach (var inv in onStart.GetInvocationList())
                        {
                            try
                            {
                                var _onStart = (StartEventHandler)inv;
                                _onStart(this);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Invoke Delegate OnStart Catch Exception \n" + e);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Listened = false;
                Console.WriteLine(e);
            }
        }

        public void Stop()
        {
            locker.EnterWriteLock();

            if (Listened)
            {
                try
                {
                    Listened = false;

                    if (acceptThread != null && acceptThread.IsAlive)
                    {
                        acceptThread.Abort();
                    }

                    if (onStop != null)
                    {
                        //执行所有接受委托事件
                        foreach (var inv in onStop.GetInvocationList())
                        {
                            try
                            {
                                var _onStop = (StopEventHandler)inv;
                                _onStop(this);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Invoke Delegate OnStop Catch Exception \n" + e);
                            }
                        }
                    }

                    workSocket.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            locker.ExitWriteLock();
        }

    }
}
