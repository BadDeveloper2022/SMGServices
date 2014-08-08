using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class Deliver_Resp : BaseCommand
    {
        #region fields

        /// <summary>
        /// 结果 1字节
        /// </summary>
        public uint Result { get; set; }

        #endregion

        public Deliver_Resp()
        {
            base.Command = Commands.Deliver_Resp;
        }

        public Deliver_Resp(byte[] bytes)
        {
            this.Result = bytes[HEADER_LENGTH];
        }

        public override byte[] GetBytes()
        {
            byte[] buffer = new byte[HEADER_LENGTH + 1 + 8];
            base.TotalMessageLength = (uint)buffer.Length;

            //消息头
            base.Headers.CopyTo(buffer, 0);
            //消息体
            int offset = HEADER_LENGTH;
            buffer[offset] = (byte)Result;

            return buffer;
        }
    }
}
