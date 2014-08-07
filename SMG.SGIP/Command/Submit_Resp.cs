using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class Submit_Resp : BaseCommand
    {
        #region fields

        /// <summary>
        /// 结果 1字节
        /// </summary>
        public uint Result { get; set; }

        #endregion

        public Submit_Resp()
        {
            base.Command = Commands.Submit_Resp;
        }

        public Submit_Resp(byte[] bytes)
        {
            this.Result = bytes[HEADER_LENGTH];
        }

        public override byte[] GetBytes()
        {
            byte[] buffer = new byte[HEADER_LENGTH + 1 + 8];
            base.MessageLength = (uint)buffer.Length;

            int index = 0;
            //消息头
            base.Headers.CopyTo(buffer, index);
            //消息体
            index += HEADER_LENGTH;
            buffer[index] = (byte)Result;

            return buffer;
        }

    }
}
