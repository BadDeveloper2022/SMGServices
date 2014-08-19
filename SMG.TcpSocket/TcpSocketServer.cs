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
        private event ExceptionEventHandler onException;

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

        public event ExceptionEventHandler OnException
        {
            add { onException += value; }
            remove { onException -= value; }
        }

        #endregion

        #region request events

        private void RequestConnectedEvent(TcpSocketClient client)
        {
            if (onConnected != null)
            {
                foreach (var inv in onConnected.GetInvocationList())
                {
                    var invoke = (ConnectedEventHandler)inv;
                    invoke(client);
                }
            }
        }

        private void RequestStartEvent()
        {
            if (onStart != null)
            {
                foreach (var inv in onStart.GetInvocationList())
                {
                    var invoke = (StartEventHandler)inv;
                    invoke(this);
                }
            }
        }

        private void RequestStopEvent()
        {
            if (onStop != null)
            {
                foreach (var inv in onStop.GetInvocationList())
                {
                    var invoke = (StopEventHandler)inv;
                    invoke(this);
                }
            }
        }

        private void RequestExceptionEvent(Exception e)
        {
            if (onException != null)
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
                                        tcpClient.OnException += onException;
                                        tcpClient.Start();

                                        //调用委托事件
                                        RequestConnectedEvent(tcpClient);
                                    }
                                }
                                catch (Exception e)
                                {
                                    RequestExceptionEvent(e);
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
                    //调用委托事件
                    RequestStartEvent();
                }
            }
            catch (Exception e)
            {
                Listened = false;
                RequestExceptionEvent(e);
            }
        }

        public void Stop()
        {
            locker.EnterWriteLock();

            if (Listened)
            {
                try
                {
                    if (acceptThread != null && acceptThread.IsAlive)
                    {
                        acceptThread.Abort();
                    }
                    //调用委托事件
                    RequestStopEvent();
                    workSocket.Close();
                }
                catch (Exception e)
                {
                    RequestExceptionEvent(e);
                }
                finally
                {
                    Listened = false;
                }
            }

            locker.ExitWriteLock();
        }

    }
}
