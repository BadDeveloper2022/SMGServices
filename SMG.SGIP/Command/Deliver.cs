using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class Deliver : BaseCommand
    {
        #region Properties

        /// <summary>
        /// 发送短消息的用户手机号，手机号码前加“86”国别标志  21字节
        /// </summary>
        public string UserNumber { get; set; }

        /// <summary>
        /// SP的接入号码  21字节
        /// </summary>
        public string SPNumber { get; set; }

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
        /// 短消息的长度 4字节
        /// </summary>
        public uint MessageLength { get; set; }

        /// <summary>
        ///短消息的内容  MessageLength字节
        /// </summary>
        public string MessageContent { get; set; }

        #endregion

        public Deliver()
        {
            base.Command = Commands.Deliver;
            base.SequenceNumber = Sequence.Next();
        }

        public Deliver(byte[] bytes)
            : base(bytes)
        {
            try
            {
                int offset = HEADER_LENGTH;
                this.UserNumber = GetString(bytes, offset, 21);
                offset += 21;
                this.SPNumber = GetString(bytes, offset, 21);
                offset += 21;
                this.TP_pid = bytes[offset];
                offset++;
                this.TP_udhi = bytes[offset];
                offset++;
                this.MessageCoding = bytes[offset];
                offset++;
                this.MessageLength = ToUInt32(bytes, offset, 4);
                offset += 4;
                switch (this.MessageCoding)
                {
                    case MessageCodes.ASIIC:
                        this.MessageContent = GetString(bytes, offset, (int)this.MessageLength);
                        break;
                    case MessageCodes.UCS2:
                        this.MessageContent = GetString(Encoding.GetEncoding("utf-16"), bytes, offset, (int)this.MessageLength);
                        break;
                    case MessageCodes.GBK:
                        this.MessageContent = GetString(Encoding.BigEndianUnicode, bytes, offset, (int)this.MessageLength);
                        break;
                    default:
                        this.MessageContent = GetString(Encoding.Default, bytes, offset, (int)this.MessageLength);
                        break;
                }
            }
            catch
            {
                throw new BadCmdBodyException(Commands.Deliver);
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
                byte[] unbts = GetBytes(UserNumber);
                Array.Copy(unbts, 0, bytes, offset, unbts.Length);
                offset += 21;
                byte[] snbts = GetBytes(SPNumber);
                Array.Copy(snbts, 0, bytes, offset, snbts.Length);
                offset += 21;
                bytes[offset] = (byte)TP_pid;
                offset++;
                bytes[offset] = (byte)TP_udhi;
                offset++;
                bytes[offset] = (byte)MessageCoding;
                offset++;
                byte[] mcbts = null;
                switch (this.MessageCoding)
                {
                    case MessageCodes.ASIIC:
                        mcbts = GetBytes(this.MessageContent);
                        break;
                    case MessageCodes.UCS2:
                        mcbts = GetBytes(Encoding.GetEncoding("utf-16"), this.MessageContent);
                        break;
                    case MessageCodes.GBK:
                        mcbts = GetBytes(Encoding.BigEndianUnicode, this.MessageContent);
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
                throw new BadCmdBodyException(Commands.Deliver);
            }

            //发送实际的字节流
            byte[] res = new byte[offset];
            bytes.CopyTo(res, 0);

            return res;
        }
    }
}
