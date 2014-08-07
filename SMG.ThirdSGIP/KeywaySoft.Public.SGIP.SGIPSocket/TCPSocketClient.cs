namespace KeywaySoft.Public.SGIP.SGIPSocket
{
    using KeywaySoft.Public.SGIP;
    using KeywaySoft.Public.SGIP.Base;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class TCPSocketClient
    {
        private const int DefaultBufferSize = 0x10000;
        private string m_IP;
        private bool m_IsRun;
        private bool m_IsSend;
        private DateTime m_LastTime;
        private int m_LoginType;
        private string m_Password;
        private int m_Port;
        private Thread m_Recv;
        private byte[] m_RecvDataBuffer = new byte[0x10000];
        private Socket m_Soc;
        private string m_UserName;

        public event LoginDelegate LoginEvent;

        private event LoginDelegate m_LoginEvent;

        private event NetEventDelegate m_RecvDataEvent;

        public event NetEventDelegate RecvDataEvent;

        public TCPSocketClient(string ip, int port)
        {
            this.m_IP = ip;
            this.m_Port = port;
        }

        public void CloseSocket()
        {
            this.m_IsRun = false;
            this.m_IsSend = false;
            try
            {
                try
                {
                    this.m_Soc.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
            }
            finally
            {
                this.m_Soc.Close();
                this.m_Recv.Abort();
            }
        }

        public void Connect()
        {
            if (!this.IsConnected)
            {
                if (this.ConnectToSMG())
                {
                    this.Login();
                    this.Start();
                    this.m_IsRun = true;
                }
                else
                {
                    this.CloseSocket();
                }
            }
        }

        public bool ConnectToSMG()
        {
            bool flag = false;
            this.m_Soc = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                this.m_Soc.Connect(IPAddress.Parse(this.m_IP), this.m_Port);
                flag = true;
            }
            catch (SocketException exception)
            {
                //TODO: screen Á¬½ÓSMG
                throw exception;
            }
            return flag;
        }

        public bool Login()
        {
            try
            {
                this.SendData(new Bind(this.m_LoginType, this.m_UserName, this.m_Password).toBytes());
                int count = this.m_Soc.Receive(this.m_RecvDataBuffer);
                if (count > 0)
                {
                    byte[] dst = new byte[count];
                    Buffer.BlockCopy(this.m_RecvDataBuffer, 0, dst, 0, count);
                    //if (this.m_LoginEvent != null)
                    //{
                    //    return this.m_LoginEvent(dst);
                    //}

                    // TODO: screen
                    if (this.LoginEvent != null)
                    {
                        return this.LoginEvent(dst);
                    }

                   

                }
            }
            catch (SocketException exception)
            {
                //TODO: screen 
                throw exception;
            }
            return false;
        }

        public void RecvData()
        {
            try
            {
                // screen: TODO
                while (this.m_IsRun && this.m_Soc.Connected)
                //while (this.m_Soc.Connected)
                {
                    int recvCount = this.m_Soc.Receive(this.m_RecvDataBuffer);
                    if (recvCount == 0)
                    {
                        this.CloseSocket();
                        return;
                    }
                    //if (this.m_RecvDataEvent != null)
                    //{
                    //    this.m_RecvDataEvent(this.m_RecvDataBuffer, recvCount, this.m_Soc);
                    //}
                    // TODO: screen
                    if (this.RecvDataEvent != null)
                    {
                        this.RecvDataEvent(this.m_RecvDataBuffer, recvCount, this.m_Soc);
                    }
                    this.m_LastTime = DateTime.Now;
                }
            }
            catch (SocketException)
            {
                this.CloseSocket();
            }
        }

        public void SendData(byte[] bs)
        {
            try
            {
                if (!this.IsConnected)
                {
                    this.Connect();
                }
                this.m_Soc.Send(bs, SocketFlags.None);
                this.m_LastTime = DateTime.Now;
                this.m_IsSend = true;
            }
            catch (SocketException exception)
            {
                //TODO: screen 
                throw exception;
            }
            catch (NullReferenceException exception2)
            {
                //TODO: screen 
                throw exception2;
            }
            catch (ObjectDisposedException exception3)
            {
                //TODO: screen 
                throw exception3;
            }
        }

        public void Start()
        {
            if (this.m_Recv != null)
            {
                try
                {
                    this.m_Recv.Abort();
                }
                catch (Exception exception)
                {
                    //TODO: screen 
                   throw exception;
                }
            }
            this.m_IsSend = false;
            this.m_Recv = new Thread(new ThreadStart(this.RecvData));
            this.m_Recv.IsBackground = true;
            this.m_Recv.Start();
        }

        public void Stop()
        {
            this.CloseSocket();
        }

        public bool IsConnected
        {
            get
            {
                if (this.m_Soc == null)
                {
                    return false;
                }
                return this.m_Soc.Connected;
            }
        }

        public bool IsSend
        {
            get
            {
                return this.m_IsSend;
            }
            set
            {
                this.m_IsSend = value;
            }
        }

        public DateTime LastTime
        {
            get
            {
                return this.m_LastTime;
            }
        }

        public int LoginType
        {
            set
            {
                this.m_LoginType = value;
            }
        }

        public string Password
        {
            set
            {
                this.m_Password = value;
            }
        }

        public string UserName
        {
            set
            {
                this.m_UserName = value;
            }
        }
    }
}

