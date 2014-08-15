using SMG.SGIP.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Emulator.Phone
{
    public class SMSHistory
    {
        static Dictionary<string, List<SMS>> history;
        static ReaderWriterLockSlim locker;

        static SMSHistory()
        {
            history = new Dictionary<string, List<SMS>>();
            locker = new ReaderWriterLockSlim();
        }

        public static void Add(SMS sms)
        {
            locker.EnterWriteLock();

            if (history.ContainsKey(sms.SPNumber))
            {
                var list = history.First(i => i.Key.Equals(sms.SPNumber));
                list.Value.Add(sms);
            }
            else
            {
                history.Add(sms.SPNumber, new List<SMS> { sms });
            }

            locker.ExitWriteLock();
        }

        public static IList<SMS> GetSession(string spNumber)
        {
            if (history.ContainsKey(spNumber))
            {
                return history[spNumber];
            }

            return null;
        }

        public static IList<string> GetSessions()
        {
            return history.Keys.ToList();
        }

    }
}
