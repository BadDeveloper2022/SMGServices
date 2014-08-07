namespace KeywaySoft.Public.SGIP.Base
{
    using System;

    public class QueueItem
    {
        private int m_FailedCount = 0;
        private DateTime m_inQueueTime;
        private object m_msgObj;
        private uint m_msgState;
        private uint m_msgType;
        private uint m_Sequence;
        private int m_SqlID;

        public QueueItem(uint seq)
        {
            this.m_Sequence = seq;
            this.m_inQueueTime = DateTime.Now;
            this.m_FailedCount = 0;
        }

        public int FailedCount
        {
            get
            {
                return this.m_FailedCount;
            }
            set
            {
                this.m_FailedCount = value;
            }
        }

        public DateTime inQueueTime
        {
            get
            {
                return this.m_inQueueTime;
            }
            set
            {
                this.m_inQueueTime = value;
            }
        }

        public object msgObj
        {
            get
            {
                return this.m_msgObj;
            }
            set
            {
                this.m_msgObj = value;
            }
        }

        public uint msgState
        {
            get
            {
                return this.m_msgState;
            }
            set
            {
                this.m_msgState = value;
            }
        }

        public uint msgType
        {
            get
            {
                return this.m_msgType;
            }
            set
            {
                this.m_msgType = value;
            }
        }

        public uint Sequence
        {
            get
            {
                return this.m_Sequence;
            }
            set
            {
                this.m_Sequence = value;
            }
        }

        public int SqlID
        {
            get
            {
                return this.m_SqlID;
            }
            set
            {
                this.m_SqlID = value;
            }
        }
    }
}

