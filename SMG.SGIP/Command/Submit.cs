using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class Submit : BaseCommand
    {
        #region Properties

        /// <summary>
        /// SP的接入号码 21字节
        /// </summary>
        public string SPNumber { get; set; }

        /// <summary>
        /// 付费号码，手机号码前加“86”国别标志  21字节
        /// </summary>
        public string ChargeNumber { get; set; }

        /// <summary>
        /// 接收短消息的手机数量，取值范围1至100 1字节
        /// </summary>
        public uint UserCount { get; set; }

        /// <summary>
        /// 接收该短消息的手机号，该字段重复UserCount指定的次数，手机号码前加“86”国别标志  21字节
        /// </summary>
        public string UserNumber { get; set; }

        /// <summary>
        /// 企业代码，取值范围0-99999  5字节
        /// </summary>
        public string CorpId { get; set; }

        /// <summary>
        /// 业务代码，由SP定义 10字节
        /// </summary>
        public string ServiceType { get; set; }

        /// <summary>
        /// 计费类型 1字节
        /// </summary>
        public uint FeeType { get; set; }

        /// <summary>
        /// 取值范围0-99999，该条短消息的收费值，单位为分  6字节
        /// </summary>
        public string FeeValue { get; set; }

        /// <summary>
        /// 取值范围0-99999，赠送用户的话费，单位为分  6字节
        /// </summary>
        public string GivenValue { get; set; }

        /// <summary>
        /// 代收费标志，0：应收；1：实收 1字节
        /// </summary>
        public uint AgentFlag { get; set; }

        /// <summary>
        /// 引起MT消息的原因 1字节
        ///    0-MO点播引起的第一条MT消息；
        ///    1-MO点播引起的非第一条MT消息；
        ///    2-非MO点播引起的MT消息；
        ///    3-系统反馈引起的MT消息。
        /// </summary>
        public uint MorelatetoMTFlag { get; set; }

        /// <summary>
        /// 优先级0-9从低到高，默认为0  1字节
        /// </summary>
        public uint Priority { get; set; }

        /// <summary>
        /// 短消息寿命的终止时间，如果为空，表示使用短消息中心的缺省值。 16字节
        /// 时间内容为16个字符，格式为”yymmddhhmmsstnnp” ，其中“tnnp”取固定值“032+”
        /// </summary>
        public string ExpireTime { get; set; }

        /// <summary>
        /// 短消息定时发送的时间，如果为空，表示立刻发送该短消息。 16字节
        /// 时间内容为16个字符，格式为“yymmddhhmmsstnnp” ，其中“tnnp”取固定值“032+”
        /// </summary>
        public string ScheduleTime { get; set; }

        /// <summary>
        /// 状态报告标记  1字节
        /// 0-该条消息只有最后出错时要返回状态报告
        /// 1-该条消息无论最后是否成功都要返回状态报告
        /// 2-该条消息不需要返回状态报告
        /// 3-该条消息仅携带包月计费信息，不下发给用户，要返回状态报告
        /// 其它-保留
        /// 缺省设置为0
        /// </summary>
        public uint ReportFlag { get; set; }

        /// <summary>
        /// GSM协议类型。  1字节
        /// </summary>
        public uint TP_pid { get; set; }

        /// <summary>
        /// GSM协议类型。仅使用1位，右对齐  1字节
        /// </summary>
        public uint TP_udhi { get; set; }

        /// <summary>
        /// 短消息的编码格式。 1字节
        /// 0：纯ASCII字符串
        /// 3：写卡操作
        /// 4：二进制编码
        /// 8：UCS2编码
        /// 15: GBK编码
        /// 其它参见GSM3.38第4节：SMS Data Coding Scheme
        /// </summary>
        public uint MessageCoding { get; set; }

        /// <summary>
        /// 信息类型：0-短消息信息 其它：待定  1字节
        /// </summary>
        public uint MessageType { get; set; }

        /// <summary>
        /// 短消息的长度 4字节
        /// </summary>
        public uint MessageLength { get; set; }

        /// <summary>
        ///短消息的内容  MessageLength字节
        /// </summary>
        public string MessageContent { get; set; }


        #endregion

        public Submit()
        {
            base.Command = Commands.Submit;
            base.SequenceNumber = Sequence.Next();
        }

        public Submit(byte[] bytes)
            : base(bytes)
        {
            try
            {
                int offset = HEADER_LENGTH;
                this.SPNumber = GetString(bytes, offset, 21);
                offset += 21;
                this.ChargeNumber = GetString(bytes, offset, 21);
                offset += 21;
                this.UserCount = bytes[offset];
                offset++;
                this.UserNumber = GetString(bytes, offset, 21);
                offset += 21;
                this.CorpId = GetString(bytes, offset, 5);
                offset += 5;
                this.ServiceType = GetString(bytes, offset, 10);
                offset += 10;
                this.FeeType = bytes[offset];
                offset++;
                this.FeeValue = GetString(bytes, offset, 6);
                offset += 6;
                this.GivenValue = GetString(bytes, offset, 6);
                offset += 6;
                this.AgentFlag = bytes[offset];
                offset++;
                this.MorelatetoMTFlag = bytes[offset];
                offset++;
                this.Priority = bytes[offset];
                offset++;
                this.ExpireTime = GetString(bytes, offset, 16);
                offset += 16;
                this.ScheduleTime = GetString(bytes, offset, 16);
                offset += 16;
                this.ReportFlag = bytes[offset];
                offset++;
                this.TP_pid = bytes[offset];
                offset++;
                this.TP_udhi = bytes[offset];
                offset++;
                this.MessageCoding = bytes[offset];
                offset++;
                this.MessageType = bytes[offset];
                offset++;
                this.MessageLength = ToUInt32(bytes, offset, 4);
                offset += 4;
                switch (this.MessageCoding)
                {
                    case MessageCodes.ASIIC:
                        this.MessageContent = GetString(bytes, offset, (int)this.MessageLength);
                        break;
                    case MessageCodes.UCS2:
                        this.MessageContent = GetString(Encoding.GetEncoding("UTF-16"), bytes, offset, (int)this.MessageLength);
                        break;
                    case MessageCodes.GBK:
                        this.MessageContent = GetString(Encoding.GetEncoding("GBK"), bytes, offset, (int)this.MessageLength);
                        break;
                    default:
                        this.MessageContent = GetString(Encoding.Default, bytes, offset, (int)this.MessageLength);
                        break;
                }
            }
            catch
            {
                throw new BadCmdBodyException(Commands.Submit);
            }
        }

        public override byte[] GetBytes()
        {
            //2K length
            byte[] bytes = new byte[0x800];

            //消息体
            int offset = HEADER_LENGTH;
            try
            {
                byte[] snbts = GetBytes(SPNumber);
                Array.Copy(snbts, 0, bytes, offset, snbts.Length);
                offset += 21;
                byte[] cnbts = GetBytes(ChargeNumber);
                Array.Copy(cnbts, 0, bytes, offset, cnbts.Length);
                offset += 21;
                bytes[offset] = (byte)UserCount;
                offset++;
                byte[] unbts = GetBytes(UserNumber);
                Array.Copy(unbts, 0, bytes, offset, unbts.Length);
                offset += 21;
                byte[] cpbts = GetBytes(CorpId);
                Array.Copy(cpbts, 0, bytes, offset, cpbts.Length);
                offset += 5;
                byte[] stbts = GetBytes(ServiceType);
                Array.Copy(stbts, 0, bytes, offset, stbts.Length);
                offset += 10;
                bytes[offset] = (byte)FeeType;
                offset++;
                byte[] fvbts = GetBytes(FeeValue);
                Array.Copy(fvbts, 0, bytes, offset, fvbts.Length);
                offset += 6;
                byte[] gvbts = GetBytes(GivenValue);
                Array.Copy(gvbts, 0, bytes, offset, gvbts.Length);
                offset += 6;
                bytes[offset] = (byte)AgentFlag;
                offset++;
                bytes[offset] = (byte)MorelatetoMTFlag;
                offset++;
                bytes[offset] = (byte)Priority;
                offset++;
                if (!string.IsNullOrEmpty(ExpireTime))
                {
                    byte[] etbts = GetBytes(ExpireTime);
                    Array.Copy(etbts, 0, bytes, offset, etbts.Length);
                }
                offset += 16;
                if (!string.IsNullOrEmpty(ScheduleTime))
                {
                    byte[] sctbts = GetBytes(ScheduleTime);
                    Array.Copy(sctbts, 0, bytes, offset, sctbts.Length);
                }
                offset += 16;
                bytes[offset] = (byte)ReportFlag;
                offset++;
                bytes[offset] = (byte)TP_pid;
                offset++;
                bytes[offset] = (byte)TP_udhi;
                offset++;
                bytes[offset] = (byte)MessageCoding;
                offset++;
                bytes[offset] = (byte)MessageType;
                offset++;
                byte[] mcbts = null;
                switch (this.MessageCoding)
                {
                    case MessageCodes.ASIIC:
                        mcbts = GetBytes(this.MessageContent);
                        break;
                    case MessageCodes.UCS2:
                        mcbts = GetBytes(Encoding.GetEncoding("UTF-16"), this.MessageContent);
                        break;
                    case MessageCodes.GBK:
                        mcbts = GetBytes(Encoding.GetEncoding("GBK"), this.MessageContent);
                        break;
                    default:
                        mcbts = GetBytes(Encoding.Default, this.MessageContent);
                        break;
                }
                this.MessageLength = (uint)mcbts.Length;
                byte[] mlbts = GetBytes(this.MessageLength);
                Array.Copy(mlbts, 0, bytes, offset, mlbts.Length);
                offset += 4;
                Array.Copy(mcbts, 0, bytes, offset, mcbts.Length);
                offset += mcbts.Length;
                //保留字段
                offset += 8;

                //消息头
                base.TotalMessageLength = (uint)offset;
                base.Headers.CopyTo(bytes, 0);
            }
            catch
            {
                throw new BadCmdBodyException(Commands.Submit);
            }
            //发送实际的字节流
            byte[] res = new byte[offset];
            bytes.CopyTo(res, 0);

            return res;
        }

    }
}
