namespace KeywaySoft.Public.SGIP.Base
{
    using System;
    using System.Diagnostics;

    public class QueueList : BaseSortedList<uint, QueueItem>
    {
        public QueueItem Find(uint key)
        {
            if (base.m_List.ContainsKey(key))
            {
                return base.m_List[key];
            }
            return null;
        }

        public QueueItem GetTopOutQueue()
        {
            for (int i = 0; i < base.m_List.Count; i++)
            {
                QueueItem item = base.m_List.Values[i];
                if (item.msgState == 0)
                {
                    lock (this)
                    {
                        item.msgState = 2;
                    }
                    return item;
                }
            }
            return null;
        }
    }
}

