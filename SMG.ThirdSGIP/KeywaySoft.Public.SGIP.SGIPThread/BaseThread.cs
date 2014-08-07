namespace KeywaySoft.Public.SGIP.SGIPThread
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class BaseThread
    {
        protected bool m_IsRun;
        protected int m_Num;
        protected Thread m_th;

        protected event ThreadHandle m_ThreadEvent;

        public event ThreadHandle ThreadEvent;

        public BaseThread(int num)
        {
            this.m_Num = num;
        }

        protected virtual void BeginWork()
        {
            while (this.m_IsRun)
            {
                //if (this.m_ThreadEvent != null)
                //{
                //    this.m_ThreadEvent();
                //}
                // TODO: SCREEN
                if (this.ThreadEvent != null)
                {
                    this.ThreadEvent();
                }
                Thread.Sleep(this.m_Num);
            }
        }

        public virtual void StartThread()
        {
            this.m_IsRun = true;
            this.m_th = new Thread(new ThreadStart(this.BeginWork));
            this.m_th.Start();
        }

        public virtual void StopThread()
        {
            this.m_IsRun = false;
            this.m_th.Abort();
        }
    }
}

