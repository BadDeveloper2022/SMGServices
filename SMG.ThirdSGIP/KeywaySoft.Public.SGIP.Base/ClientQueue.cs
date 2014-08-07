namespace KeywaySoft.Public.SGIP.Base
{
    using System;
    using System.Net.Sockets;

    public class ClientQueue
    {
        private Socket m_Soc;
        private DateTime m_Time;

        public ClientQueue(Socket soc, DateTime time)
        {
            this.m_Soc = soc;
            this.m_Time = time;
        }

        public Socket Soc
        {
            get
            {
                return this.m_Soc;
            }
        }

        public DateTime Time
        {
            get
            {
                return this.m_Time;
            }
            set
            {
                this.m_Time = value;
            }
        }
    }
}

