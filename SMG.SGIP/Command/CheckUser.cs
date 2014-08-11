using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class CheckUser : BaseCommand
    {
        #region Properties

        /// <summary>
        /// 计费中心给SMG分配的用户名  16字节
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 和用户名对应的密码  16字节
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 待鉴权的手机号码，手机号码前加“86”国别标志  21字节
        /// </summary>
        public string UserNumber { get; set; }

        #endregion

        public CheckUser()
        {
            base.Command = Commands.CheckUser;
            base.SequenceNumber = Sequence.Next();
        }

        public CheckUser(byte[] bytes)
            : base(bytes)
        {
            try
            {
                int offset = HEADER_LENGTH;
                this.UserName = GetString(bytes, offset, 16);
                offset += 16;
                this.Password = GetString(bytes, offset, 16);
                offset += 16;
                this.UserNumber = GetString(bytes, offset, 21);
            }
            catch
            {
                throw new BadCmdBodyException(Commands.CheckUser);
            }
        }

        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[HEADER_LENGTH + 16 + 16 + 21 + 8];
            base.TotalMessageLength = (uint)bytes.Length;

            try
            {
                //消息头
                base.Headers.CopyTo(bytes, 0);
                //消息体
                int offset = HEADER_LENGTH;
                byte[] unbts = GetBytes(UserName);
                Array.Copy(unbts, 0, bytes, offset, unbts.Length);
                offset += 16;
                byte[] pwdbts = GetBytes(Password);
                Array.Copy(pwdbts, 0, bytes, offset, pwdbts.Length);
                offset += 16;
                byte[] unbbts = GetBytes(UserNumber);
                Array.Copy(unbbts, 0, bytes, offset, unbbts.Length);
            }
            catch
            {
                throw new BadCmdBodyException(Commands.CheckUser);
            }

            return bytes;
        }
    }
}
