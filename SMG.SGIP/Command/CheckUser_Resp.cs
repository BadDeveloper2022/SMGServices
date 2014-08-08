using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class CheckUser_Resp : BaseCommand
    {
        #region fields

        /// <summary>
        /// 鉴权结果 0：鉴权成功 其它：错误码 1字节
        /// </summary>
        public uint Result { get; set; }

        /// <summary>
        ///用户状态 0：注销；1：欠费停机；2：正常  1字节
        /// </summary>
        public uint Status { get; set; }

        #endregion

        public CheckUser_Resp()
        {
            base.Command = Commands.CheckUser_Resp;
        }

        public CheckUser_Resp(byte[] bytes)
            : base(bytes)
        {
            int offset = HEADER_LENGTH;
            this.Result = bytes[offset];
            offset++;
            this.Status = bytes[offset];
        }

        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[HEADER_LENGTH + 1 + 1 + 8];
            base.TotalMessageLength = (uint)bytes.Length;

            //消息头
            base.Headers.CopyTo(bytes, 0);
            //消息体
            int offset = HEADER_LENGTH;
            bytes[offset] = (byte)Result;
            offset++;
            bytes[offset] = (byte)Status;

            return bytes;
        }
    }
}
