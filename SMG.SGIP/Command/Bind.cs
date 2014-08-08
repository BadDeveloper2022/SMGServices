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
        public string LoginName { get; set; }

        /// <summary>
        /// 登录密码 16字节
        /// </summary>
        public string LoginPassword { get; set; }

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
            this.LoginName = base.GetString(bytes, index, 16);
            this.LoginPassword = base.GetString(bytes, index + 16, 16);
        }

        public override byte[] GetBytes()
        {
            byte[] buffer = new byte[HEADER_LENGTH + 1 + 16 + 16 + 8];
            base.TotalMessageLength = (uint)buffer.Length;

            //消息头
            base.Headers.CopyTo(buffer, 0);
            //消息体
            int offset = HEADER_LENGTH;
            buffer[offset] = (byte)LoginType;
            offset++;
            byte[] ubytes = base.GetBytes(LoginName);
            Array.Copy(ubytes, 0, buffer, offset, ubytes.Length);
            offset += 16;
            byte[] pbytes = base.GetBytes(LoginPassword);
            Array.Copy(pbytes, 0, buffer, offset, pbytes.Length);

            return buffer;
        }

    }
}
