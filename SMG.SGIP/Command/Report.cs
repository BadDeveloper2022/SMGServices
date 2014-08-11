using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class Report : BaseCommand
    {
        #region Properties

        /// <summary>
        /// 该命令所涉及的Submit或deliver命令的序列号  12字节
        /// </summary>
        public uint SubmitSequenceNumber { get; set; }

        /// <summary>
        /// Report命令类型 1字节
        /// 0：对先前一条Submit命令的状态报告
        /// 1：对先前一条前转Deliver命令的状态报告
        /// </summary>
        public uint ReportType { get; set; }

        /// <summary>
        /// 接收短消息的手机号，手机号码前加“86”国别标志  21字节
        /// </summary>
        public string UserNumber { get; set; }

        /// <summary>
        /// 该命令所涉及的短消息的当前执行状态 1字节
        /// 0：发送成功
        /// 1：等待发送
        /// 2：发送失败
        /// </summary>
        public uint State { get; set; }

        /// <summary>
        /// 当State=2时为错误码值，否则为0 1字节
        /// </summary>
        public uint ErrorCode { get; set; }

        #endregion


        public Report()
        {
            base.Command = Commands.Report;
            base.SequenceNumber = Sequence.Next();
        }

        public Report(byte[] bytes)
            : base(bytes)
        {
            try
            {
                int offset = HEADER_LENGTH;
                this.SubmitSequenceNumber = ToUInt32(bytes, offset, 12);
                offset += 12;
                this.ReportType = bytes[offset];
                offset++;
                this.UserNumber = GetString(bytes, offset, 12);
                offset += 12;
                this.State = bytes[offset];
                offset++;
                this.ErrorCode = bytes[offset];
            }
            catch
            {
                throw new BadCmdBodyException(Commands.Report);
            }
        }

        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[HEADER_LENGTH + 12 + 1 + 21 + 1 + 1 + 8];
            base.TotalMessageLength = (uint)bytes.Length;

            try
            {
                //消息头
                base.Headers.CopyTo(bytes, 0);
                //消息体
                int offset = HEADER_LENGTH;
                byte[] ssnbts = GetBytes(SubmitSequenceNumber);
                Array.Copy(ssnbts, 0, bytes, offset, ssnbts.Length);
                offset += 12;
                bytes[offset] = (byte)ReportType;
                offset++;
                byte[] unbts = GetBytes(UserNumber);
                Array.Copy(unbts, 0, bytes, offset, unbts.Length);
                offset += 21;
            }
            catch
            {
                throw new BadCmdBodyException(Commands.Report);
            }

            return bytes;
        }

    }
}
