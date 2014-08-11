using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class UserRpt : BaseCommand
    {
        #region Properties

        /// <summary>
        /// SP的接入号码 21字节
        /// </summary>
        public string SPNumber { get; set; }

        /// <summary>
        /// 待配置的手机号码，手机号码前加“86”国别标志  21字节
        /// </summary>
        public string UserNumber { get; set; }

        /// <summary>
        /// 0：注销；1：欠费停机；2：恢复正常  1字节
        /// </summary>
        public uint UserCondition { get; set; }

        #endregion

        public UserRpt()
        {
            base.Command = Commands.UserRpt;
            base.SequenceNumber = Sequence.Next();
        }

        public UserRpt(byte[] bytes)
            : base(bytes)
        {
            try
            {
                int offset = HEADER_LENGTH;
                this.SPNumber = GetString(bytes, offset, 21);
                offset += 21;
                this.UserNumber = GetString(bytes, offset, 21);
                offset += 21;
                this.UserCondition = bytes[offset];
            }
            catch
            {
                throw new BadCmdBodyException(Commands.UserRpt);
            }
        }

        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[HEADER_LENGTH + 21 + 21 + 1 + 8];
            base.TotalMessageLength = (uint)bytes.Length;

            try
            {
                //消息头
                base.Headers.CopyTo(bytes, 0);
                //消息体
                int offset = HEADER_LENGTH;
                byte[] snbts = GetBytes(SPNumber);
                Array.Copy(snbts, 0, bytes, offset, snbts.Length);
                offset += 21;
                byte[] unbts = GetBytes(UserNumber);
                Array.Copy(unbts, 0, bytes, offset, unbts.Length);
                offset += 21;
                bytes[offset] = (byte)UserCondition;
            }
            catch
            {
                throw new BadCmdBodyException(Commands.UserRpt);
            }

            return bytes;
        }

    }
}
