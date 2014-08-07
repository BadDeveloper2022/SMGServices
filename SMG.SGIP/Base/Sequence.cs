using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SMG.SGIP.Base
{
    public class Sequence
    {
        private static byte[] nodeBytes = new byte[4];
        private static uint ordinal = 0;
        private static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        /// <summary>
        /// 获取发送时间 格式为 mmddhhmmss
        /// </summary>
        private static uint CurrentTime
        {
            get
            {
                string month = DateTime.Now.Month.ToString().PadLeft(2, '0');
                string day = DateTime.Now.Day.ToString().PadLeft(2, '0');
                string hour = DateTime.Now.Hour.ToString().PadLeft(2, '0');
                string minute = DateTime.Now.Minute.ToString().PadLeft(2, '0');
                string second = DateTime.Now.Second.ToString().PadLeft(2, '0');

                return uint.Parse(month + day + hour + minute + second);
            }
        }

        /// <summary>
        /// 获取递增序列号
        /// </summary>
        private static uint CurrentOrdinal
        {
            get
            {
                try
                {
                    ordinal++;
                }
                catch (OverflowException)
                {
                    ordinal = uint.MaxValue;
                }

                return ordinal;
            }
        }

        /// <summary>
        /// 设置节点号码
        /// </summary>
        /// <param name="nodeNumber"></param>
        public static void SetNodeNumber(uint nodeNumber)
        {
            NetBitConverter.GetBytes(nodeNumber).CopyTo(nodeBytes, 0);
        }

        public static byte[] Next()
        {
            byte[] seqs = new byte[12];

            locker.EnterWriteLock();

            Array.Copy(nodeBytes, 0, seqs, 0, 4);
            Array.Copy(NetBitConverter.GetBytes(CurrentTime), 0, seqs, 4, 4);
            Array.Copy(NetBitConverter.GetBytes(CurrentOrdinal), 0, seqs, 8, 4);

            locker.ExitWriteLock();

            return seqs;
        }

    }
}
