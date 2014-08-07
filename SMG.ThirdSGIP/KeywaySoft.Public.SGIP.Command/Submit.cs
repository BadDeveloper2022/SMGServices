namespace KeywaySoft.Public.SGIP.Command
{
    using KeywaySoft.Public.SGIP.Base;
    using System;
    using System.Text;

    public class Submit : BaseCommand
    {
        private int m_agentFlag;
        private string m_chargeNumber;
        private string m_corpId;
        private string m_expireTime;
        private int m_feeType;
        private string m_feeValue;
        private string m_givenValue;
        private byte[] m_linkID;
        private uint m_messageCoding;
        private string m_messageContent;
        private uint m_messageLength;
        private int m_messageType;
        private int m_morelatetoMTFlag;
        private int m_priority;
        private int m_reportFlag;
        private string m_scheduleTime;
        private string m_serviceType;
        private string m_spNumber;
        private int m_TP_pid;
        private int m_TP_udhi;
        private int m_userCount;
        private string m_userNumber;

        public Submit(byte[] seq) : base(3, seq)
        {
            this.m_userCount = 1;
            this.m_morelatetoMTFlag = 0;
            this.m_priority = 0;
            this.m_messageType = 0;
            this.m_linkID = new byte[8];
        }

        public Submit(byte[] bs, int msgCount) : base(bs, msgCount)
        {
            this.m_userCount = 1;
            this.m_morelatetoMTFlag = 0;
            this.m_priority = 0;
            this.m_messageType = 0;
            this.m_linkID = new byte[8];
        }

        public override byte[] toBytes()
        {
            byte[] dst = new byte[0x7d0];
            int dstOffset = 0;
            byte[] bytes = Encoding.ASCII.GetBytes(this.m_spNumber);
            Buffer.BlockCopy(bytes, 0, dst, dstOffset, bytes.Length);
            dstOffset += 0x15;
            if ((this.m_chargeNumber != null) && (this.m_chargeNumber != ""))
            {
                byte[] buffer3 = Encoding.ASCII.GetBytes(this.m_chargeNumber);
                Buffer.BlockCopy(buffer3, 0, dst, dstOffset, buffer3.Length);
            }
            dstOffset += 0x15;
            dst[dstOffset++] = (byte) this.m_userCount;
            byte[] src = Encoding.ASCII.GetBytes(this.m_userNumber);
            Buffer.BlockCopy(src, 0, dst, dstOffset, src.Length);
            dstOffset += 0x15;
            byte[] buffer5 = Encoding.ASCII.GetBytes(this.m_corpId);
            Buffer.BlockCopy(buffer5, 0, dst, dstOffset, buffer5.Length);
            dstOffset += 5;
            byte[] buffer6 = Encoding.ASCII.GetBytes(this.m_serviceType);
            Buffer.BlockCopy(buffer6, 0, dst, dstOffset, buffer6.Length);
            dstOffset += 10;
            dst[dstOffset++] = (byte) this.m_feeType;
            byte[] buffer7 = Encoding.ASCII.GetBytes(this.m_feeValue);
            Buffer.BlockCopy(buffer7, 0, dst, dstOffset, buffer7.Length);
            dstOffset += 6;
            byte[] buffer8 = Encoding.ASCII.GetBytes(this.m_givenValue);
            Buffer.BlockCopy(buffer8, 0, dst, dstOffset, buffer8.Length);
            dstOffset += 6;
            dst[dstOffset++] = (byte) this.m_agentFlag;
            dst[dstOffset++] = (byte) this.m_morelatetoMTFlag;
            dst[dstOffset++] = (byte) this.m_priority;
            if ((this.m_expireTime != null) && (this.m_expireTime != ""))
            {
                byte[] buffer9 = Encoding.ASCII.GetBytes(this.m_expireTime);
                Buffer.BlockCopy(buffer9, 0, dst, dstOffset, buffer9.Length);
            }
            dstOffset += 0x10;
            if ((this.m_scheduleTime != null) && (this.m_scheduleTime != ""))
            {
                byte[] buffer10 = Encoding.ASCII.GetBytes(this.m_scheduleTime);
                Buffer.BlockCopy(buffer10, 0, dst, dstOffset, buffer10.Length);
            }
            dstOffset += 0x10;
            dst[dstOffset++] = (byte) this.m_reportFlag;
            dst[dstOffset++] = (byte) this.m_TP_pid;
            dst[dstOffset++] = (byte) this.m_TP_udhi;
            dst[dstOffset++] = (byte) this.m_messageCoding;
            dst[dstOffset++] = (byte) this.m_messageType;
            byte[] buffer11 = null;
            uint messageCoding = this.m_messageCoding;
            if (messageCoding != 8)
            {
                if (messageCoding != 15)
                {
                    buffer11 = Encoding.ASCII.GetBytes(this.m_messageContent);
                }
                else
                {
                    buffer11 = Encoding.Default.GetBytes(this.m_messageContent);
                }
            }
            else
            {
                buffer11 = Encoding.BigEndianUnicode.GetBytes(this.m_messageContent);
            }
            Buffer.BlockCopy(BitConvert.uint2Bytes((uint) buffer11.Length), 0, dst, dstOffset, 4);
            dstOffset += 4;
            Buffer.BlockCopy(buffer11, 0, dst, dstOffset, buffer11.Length);
            dstOffset += buffer11.Length;
            Buffer.BlockCopy(this.m_linkID, 0, dst, dstOffset, 8);
            dstOffset += 8;
            byte[] array = new byte[dstOffset + MSGHead.MSGLength];
            base._MSGHead.msgLength = ((uint) dstOffset) + MSGHead.MSGLength;
            base._MSGHead.toBytes().CopyTo(array, 0);
            Buffer.BlockCopy(dst, 0, array, (int) MSGHead.MSGLength, dstOffset);
            return array;
        }

        public int AgentFlag
        {
            get
            {
                return this.m_agentFlag;
            }
            set
            {
                this.m_agentFlag = value;
            }
        }

        public string ChargeNumber
        {
            get
            {
                return this.m_chargeNumber;
            }
            set
            {
                this.m_chargeNumber = value;
            }
        }

        public string CorpId
        {
            get
            {
                return this.m_corpId;
            }
            set
            {
                this.m_corpId = value;
            }
        }

        public string ExpireTime
        {
            get
            {
                return this.m_expireTime;
            }
            set
            {
                this.m_expireTime = value;
            }
        }

        public int FeeType
        {
            get
            {
                return this.m_feeType;
            }
            set
            {
                this.m_feeType = value;
            }
        }

        public string FeeValue
        {
            get
            {
                return this.m_feeValue;
            }
            set
            {
                this.m_feeValue = value;
            }
        }

        public string GivenValue
        {
            get
            {
                return this.m_givenValue;
            }
            set
            {
                this.m_givenValue = value;
            }
        }

        public string LinkID
        {
            get
            {
                return Encoding.ASCII.GetString(this.m_linkID);
            }
            set
            {
                if ((value != null) && (value != ""))
                {
                    Array.Copy(BitConvert.uint2Bytes(uint.Parse(value.Substring(0, 4))), this.m_linkID, 4);
                    Array.Copy(BitConvert.uint2Bytes(uint.Parse(value.Substring(4, 4))), this.m_linkID, 4);
                }
            }
        }

        public uint MessageCoding
        {
            get
            {
                return this.m_messageCoding;
            }
            set
            {
                this.m_messageCoding = value;
            }
        }

        public string MessageContent
        {
            get
            {
                return this.m_messageContent;
            }
            set
            {
                this.m_messageContent = value;
            }
        }

        public uint MessageLength
        {
            get
            {
                return this.m_messageLength;
            }
        }

        public int MessageType
        {
            get
            {
                return this.m_messageType;
            }
            set
            {
                this.m_messageType = value;
            }
        }

        public int MorelatetoMTFlag
        {
            get
            {
                return this.m_morelatetoMTFlag;
            }
            set
            {
                this.m_morelatetoMTFlag = value;
            }
        }

        public int Priority
        {
            get
            {
                return this.m_priority;
            }
            set
            {
                this.m_priority = value;
            }
        }

        public int ReportFlag
        {
            get
            {
                return this.m_reportFlag;
            }
            set
            {
                this.m_reportFlag = value;
            }
        }

        public string ScheduleTime
        {
            get
            {
                return this.m_scheduleTime;
            }
            set
            {
                this.m_scheduleTime = value;
            }
        }

        public string ServiceType
        {
            get
            {
                return this.m_serviceType;
            }
            set
            {
                this.m_serviceType = value;
            }
        }

        public string SPNumber
        {
            get
            {
                return this.m_spNumber;
            }
            set
            {
                this.m_spNumber = value;
            }
        }

        public int TP_pid
        {
            get
            {
                return this.m_TP_pid;
            }
            set
            {
                this.m_TP_pid = value;
            }
        }

        public int TP_udhi
        {
            get
            {
                return this.m_TP_udhi;
            }
            set
            {
                this.m_TP_udhi = value;
            }
        }

        public int UserCount
        {
            get
            {
                return this.m_userCount;
            }
            set
            {
                this.m_userCount = value;
            }
        }

        public string UserNumber
        {
            get
            {
                return this.m_userNumber;
            }
            set
            {
                this.m_userNumber = value;
            }
        }
    }
}

