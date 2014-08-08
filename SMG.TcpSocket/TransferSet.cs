using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.TcpSocket
{
    public class TransferSet
    {
        /// <summary>
        /// 每个消息包不大于2K
        /// </summary>
        public static readonly int BufferSize = 2048;
        public static readonly string EndChar = "\0";
        public static readonly int EndByte = 0;
    }
}
