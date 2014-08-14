using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class Bind_Resp : BaseCommand
    {
        #region Properties

        /// <summary>
        /// 结果 1字节
        /// </summary>
        public uint Result { get; set; }

        #endregion

        public Bind_Resp()
        {
            base.Command = Commands.Bind_Resp;
        }

        public Bind_Resp(byte[] bytes)
            : base(bytes)
        {
            try
            {
                this.Result = bytes[HEADER_LENGTH];
            }
            catch
            {
                throw new BadCmdBodyException(Commands.Bind_Resp);
            }
        }

        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[HEADER_LENGTH + 1 + 8];
            base.TotalMessageLength = (uint)bytes.Length;

            try
            {
                //消息头
                base.Headers.CopyTo(bytes, 0);
                //消息体
                int offset = HEADER_LENGTH;
                bytes[offset] = (byte)Result;
            }
            catch
            {
                throw new BadCmdBodyException(Commands.Bind_Resp);
            }

            return bytes;
        }

    }
}
