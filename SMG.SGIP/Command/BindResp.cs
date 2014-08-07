using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class BindResp : BaseCommand
    {
        #region fields

        /// <summary>
        /// 结果 1字节
        /// </summary>
        public uint Result { get; set; }

        #endregion

        public BindResp()
        {
            base.Command = Commands.Bind_Resp;
        }

        public BindResp(byte[] bytes)
            : base(bytes)
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
