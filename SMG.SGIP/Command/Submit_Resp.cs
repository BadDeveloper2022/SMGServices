using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class Submit_Resp : BaseCommand
    {
        #region Properties

        /// <summary>
        /// 结果 1字节
        /// </summary>
        public uint Result { get; set; }

        #endregion

        public Submit_Resp()
        {
            base.Command = Commands.Submit_Resp;
        }

        public Submit_Resp(byte[] bytes) :
            base(bytes)
        {
            try
            {
                this.Result = bytes[HEADER_LENGTH];
            }
            catch
            {
                throw new BadCmdBodyException(Commands.Submit_Resp);
            }
        }

        public override byte[] GetBytes()
        {
            byte[] buffer = new byte[HEADER_LENGTH + 1 + 8];
            base.TotalMessageLength = (uint)buffer.Length;

            try
            {
                //消息头
                base.Headers.CopyTo(buffer, 0);
                //消息体
                int offset = HEADER_LENGTH;
                buffer[offset] = (byte)Result;
            }
            catch
            {
                throw new BadCmdBodyException(Commands.Submit_Resp);
            }

            return buffer;
        }

    }
}
