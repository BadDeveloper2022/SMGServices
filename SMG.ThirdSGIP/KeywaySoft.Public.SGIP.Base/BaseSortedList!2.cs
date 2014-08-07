namespace KeywaySoft.Public.SGIP.Base
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class BaseSortedList<K, V>
    {
        protected SortedList<K, V> m_List;
        protected object m_Lock;

        public BaseSortedList()
        {
            this.m_List = new SortedList<K, V>();
            this.m_Lock = new object();
        }

        public void Add(K key, V Obj)
        {
            if (!this.m_List.ContainsKey(key))
            {
                lock (this.m_Lock)
                {
                    this.m_List.Add(key, Obj);
                }
            }
        }

        public void Remove(K key)
        {
            if (this.m_List.ContainsKey(key))
            {
                lock (this.m_Lock)
                {
                    this.m_List.Remove(key);
                }
            }
        }

        public int Count
        {
            get
            {
                return this.m_List.Count;
            }
        }

        public V this[K key]
        {
            get
            {
                if (this.m_List.ContainsKey(key))
                {
                    return this.m_List[key];
                }
                return default(V);
            }
            set
            {
                lock (this.m_Lock)
                {
                    if (this.m_List.ContainsKey(key))
                    {
                        this.m_List[key] = value;
                    }
                }
            }
        }

        public SortedList<K, V> List
        {
            get
            {
                return this.m_List;
            }
        }
    }
}

