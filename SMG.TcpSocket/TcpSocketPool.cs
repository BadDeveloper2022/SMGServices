using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SMG.TcpSocket
{
    public class TcpSocketPool
    {
        private ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        private List<TcpSocketClient> pool;

        public TcpSocketPool()
        {
            pool = new List<TcpSocketClient>();
        }

        public bool Add(TcpSocketClient client)
        {
            bool b = false;
            locker.EnterWriteLock();

            if (pool.Contains(client))
            {
                pool.Add(client);
                b = true;
            }

            locker.ExitWriteLock();

            return b;
        }

        public bool Remove(TcpSocketClient client)
        {
            bool b = false;
            locker.EnterWriteLock();

            if (pool.Contains(client))
            {
                b = pool.Remove(client);
            }

            locker.ExitWriteLock();

            return b;
        }

        public List<TcpSocketClient> GetList()
        {
            return this.pool;
        }

    }
}
