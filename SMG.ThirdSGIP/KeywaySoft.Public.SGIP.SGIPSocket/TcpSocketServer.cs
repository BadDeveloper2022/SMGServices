namespace KeywaySoft.Public.SGIP.SGIPSocket
{
    using KeywaySoft.Public.SGIP;
    using KeywaySoft.Public.SGIP.Base;
    using KeywaySoft.Public.SGIP.Command;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class TcpSocketServer
    {
        private BaseSortedList<string, ClientQueue> m_ClientInfo;
        private bool m_IsRun;
        private byte[] m_MSG = new byte[0x10000];
        private ManualResetEvent m_MySet = new ManualResetEvent(false);
        private Socket m_Server;
        private string m_SPName;
        private string m_SPPassword;
        private string m_SVRIP;
        private int m_SVRPort;

        private event NetEventDelegate m_RecvDataEvent;
        public event NetEventDelegate RecvDataEvent;

        public TcpSocketServer(string svrIP, int svrPort)
        {
            this.m_SVRIP = svrIP;
            this.m_SVRPort = svrPort;
            this.m_IsRun = true;
        }

        public void AcceptCallBack(IAsyncResult ia)
        {
            this.m_MySet.Set();
            try
            {
                Socket soc = ((Socket) ia.AsyncState).EndAccept(ia);
                ClientQueue queue = new ClientQueue(soc, DateTime.Now);
                this.m_ClientInfo.Add(this.GetSocketIP(soc), queue);
                int msgCount = soc.Receive(this.m_MSG);
                if (msgCount > 0)
                {
                    int num2 = 0;
                    if (BitConvert.bytes2Uint(this.m_MSG, 0) == msgCount)
                    {
                        if (BitConvert.bytes2Uint(this.m_MSG, 4) == 1)
                        {
                            Bind bind = new Bind(this.m_MSG, msgCount);
                            if (bind.LoginType == 1)
                            {
                                if ((bind.LoginName == this.m_SPName) && (bind.LoginPassword == this.m_SPPassword))
                                {
                                    num2 = 0;
                                }
                                else
                                {
                                    num2 = 1;
                                }
                            }
                            else
                            {
                                num2 = 4;
                            }
                            BindResp resp = new BindResp(bind.SequenceNumber);
                            resp.Result = num2;
                            this.SendMSG(resp.toBytes(), soc);
                            if (num2 == 0)
                            {
                                ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(this.Recv), soc);
                            }
                        }
                    }
                    else
                    {
                        this.CloseSocket(soc);
                    }
                }
            }
            catch (SocketException exception)
            {
                //TODO: screen 
                //throw exception;
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public void CloseSocket(Socket client)
        {
            try
            {
                try
                {
                    client.Shutdown(SocketShutdown.Both);
                    this.m_ClientInfo.Remove(this.GetSocketIP(client));
                }
                catch
                {
                }
            }
            finally
            {
                client.Close();
            }
        }

        private string GetSocketIP(Socket soc)
        {
            try
            {
                IPEndPoint remoteEndPoint = (IPEndPoint) soc.RemoteEndPoint;
                return (remoteEndPoint.Address.ToString() + ":" + remoteEndPoint.Port.ToString());
            }
            catch
            {
                return "";
            }
        }

        public void Recv(object soc)
        {
            try
            {
                Socket client = (Socket) soc;
                while (this.m_IsRun && client.Connected)
                {
                    int recvCount = client.Receive(this.m_MSG);
                    if (recvCount > 0)
                    {
                        if (BitConvert.bytes2Uint(this.m_MSG, 0) != recvCount)
                        {
                            this.CloseSocket(client);
                            return;
                        }
                        ClientQueue queue = this.m_ClientInfo[this.GetSocketIP(client)];
                        queue.Time = DateTime.Now;
                        /*
                        if (this.m_RecvDataEvent != null)
                        {
                            this.m_RecvDataEvent(this.m_MSG, recvCount, client);
                        }
                         */

                        if (this.RecvDataEvent != null)
                        {
                            this.RecvDataEvent(this.m_MSG, recvCount, client);
                        }

                        //else
                        //{
                        //    this.m_RecvDataEvent = new NetEventDelegate(this.m_MSG, this.m_MSG.Length, soc);
                        //}
                    }
                    else
                    {
                        this.CloseSocket(client);
                    }
                }
            }
            catch (SocketException exception)
            {
                // TODO: screen
                //throw exception;
            }
        }

        public void SendMSG(byte[] bs, Socket soc)
        {
            try
            {
                if (soc.Send(bs) == 0)
                {
                    this.CloseSocket(soc);
                }
            }
            catch (SocketException)
            {
                this.CloseSocket(soc);
            }
        }

        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(this.StratThead));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            try
            {
                try
                {
                    this.m_Server.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
            }
            finally
            {
                this.m_Server.Close();
            }
        }

        public void StratThead()
        {
            try
            {
                this.m_ClientInfo = new BaseSortedList<string, ClientQueue>();
                this.m_Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(this.m_SVRIP), this.m_SVRPort);
                this.m_Server.Bind(localEP);
                this.m_Server.Listen(50);
                while (this.m_IsRun)
                {
                    this.m_MySet.Reset();
                    this.m_Server.BeginAccept(new AsyncCallback(this.AcceptCallBack), this.m_Server);
                    this.m_MySet.WaitOne();
                }
            }
            catch
            {
                this.Stop();
            }
        }

        public BaseSortedList<string, ClientQueue> ClientInfo
        {
            get
            {
                return this.m_ClientInfo;
            }
        }

        public string SPName
        {
            set
            {
                this.m_SPName = value;
            }
        }

        public string SPPassword
        {
            set
            {
                this.m_SPPassword = value;
            }
        }
    }
}

