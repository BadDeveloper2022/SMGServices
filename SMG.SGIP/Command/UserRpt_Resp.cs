using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class UserRpt_Resp : BaseCommand
    {
        #region fields

        /// <summary>
        /// 结果 1字节
        /// </summary>
        public uint Result { get; set; }

        #endregion

        public UserRpt_Resp()
        {
            base.Command = Commands.UserRpt_Resp;
        }

        public UserRpt_Resp(byte[] bytes)
            : base(bytes)
        {
            this.Result = bytes[HEADER_LENGTH];
        }

        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[HEADER_LENGTH + 1 + 8];
            base.TotalMessageLength = (uint)bytes.Length;

            //消息头
            base.Headers.CopyTo(bytes, 0);
            //消息体
            int offset = HEADER_LENGTH;
            bytes[offset] = (byte)Result;

            return bytes;
        }

    }
}
