using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Socket
{
    public class TransferSet
    {
        public static readonly Encoding Charset = Encoding.ASCII;
        /// <summary>
        /// 每个消息包不大于2K ，缓冲区使用4K
        /// </summary>
        public static readonly int BufferSize = 4096;
        public static readonly string EndChar = "\0";
        public static readonly int EndByte = 0;
    }
}
