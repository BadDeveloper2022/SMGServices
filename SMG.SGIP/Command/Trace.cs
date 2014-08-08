using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class Trace : BaseCommand
    {
        #region fields

        /// <summary>
        /// 被跟踪MT短消息的命令序列号 12字节
        /// </summary>
        public uint SubmitSequenceNumber { get; set; }

        /// <summary>
        /// 待配置的手机号码，手机号码前加“86”国别标志  21字节
        /// </summary>
        public string UserNumber { get; set; }

        #endregion

        public Trace()
        {
            base.Command = Commands.Trace;
            base.SequenceNumber = Sequence.Next();
        }

        public Trace(byte[] bytes):
            base(bytes)
        {
            int offset = HEADER_LENGTH;
            this.SubmitSequenceNumber = ToUInt32(bytes, offset, 12);
            offset += 12;
            this.UserNumber = GetString(bytes, offset, 21);
        }

        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[HEADER_LENGTH + 12 + 21 + 8];
            base.TotalMessageLength = (uint)bytes.Length;

            //消息头
            base.Headers.CopyTo(bytes, 0);
            //消息体
            int offset = HEADER_LENGTH;
            byte[] snbts = GetBytes(SubmitSequenceNumber);
            Array.Copy(snbts, 0, bytes, offset, snbts.Length);
            offset += 12;
            byte[] unbts = GetBytes(UserNumber);
            Array.Copy(unbts, 0, bytes, offset, unbts.Length);

            return bytes;
        }
    }
}
