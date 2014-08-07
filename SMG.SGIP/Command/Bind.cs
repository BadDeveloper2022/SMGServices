using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class Bind : BaseCommand
    {
        #region fields

        /// <summary>
        /// 登录类型 1字节
        /// </summary>
        public uint LoginType { get; set; }

        /// <summary>
        /// 登录名 16字节
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码 16字节
        /// </summary>
        public string Password { get; set; }

        #endregion

        public Bind()
        {
            base.Command = Commands.Bind;
            base.SequenceNumber = Sequence.Next();
        }

        public Bind(byte[] bytes)
            : base(bytes)
        {
            int index = HEADER_LENGTH;
            this.LoginType = bytes[index];
            index++;
            this.UserName = base.GetString(bytes, index, 16);
            this.Password = base.GetString(bytes, index + 16, 16);
        }

        public override byte[] GetBytes()
        {
            byte[] buffer = new byte[HEADER_LENGTH + 1 + 16 + 16 + 8];
            base.MessageLength = (uint)buffer.Length;

            int index = 0;
            //消息头
            base.Headers.CopyTo(buffer, index);
            //消息体
            index += HEADER_LENGTH;
            buffer[index] = (byte)LoginType;
            index++;
            byte[] ubytes = base.GetBytes(UserName);
            Array.Copy(ubytes, 0, buffer, index, ubytes.Length);
            index += 16;
            byte[] pbytes = base.GetBytes(Password);
            Array.Copy(pbytes, 0, buffer, index, pbytes.Length);

            return buffer;
        }

    }
}
