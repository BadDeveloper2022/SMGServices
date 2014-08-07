namespace KeywaySoft.Public.SGIP.Command
{
    using KeywaySoft.Public.SGIP.Base;
    using System;
    using System.Text;

    public class Deliver : BaseCommand
    {
        private byte[] m_linkID;
        private uint m_messageCoding;
        private string m_messageContent;
        private uint m_messageLength;
        private string m_SpNumber;
        private int m_tp_pid;
        private int m_tp_udhi;
        private string m_UserNumber;

        public Deliver(byte[] seq) : base(4, seq)
        {
            this.m_linkID = new byte[8];
        }

        public Deliver(byte[] bs, int msgCount) : base(bs, msgCount)
        {
            this.m_linkID = new byte[8];
            int mSGLength = (int) MSGHead.MSGLength;
            this.m_UserNumber = Encoding.ASCII.GetString(bs, mSGLength, 0x15).Replace("\0", "");
            mSGLength += 0x15;
            this.m_SpNumber = Encoding.ASCII.GetString(bs, mSGLength, 0x15).Replace("\0", "");
            mSGLength += 0x15;
            this.m_tp_pid = bs[mSGLength++];
            this.m_tp_udhi = bs[mSGLength++];
            this.m_messageCoding = bs[mSGLength++];
            this.m_messageLength = BitConvert.bytes2Uint(bs, mSGLength);
            mSGLength += 4;
            uint messageCoding = this.m_messageCoding;
            if (messageCoding != 8)
            {
                if (messageCoding != 15)
                {
                    this.m_messageContent = Encoding.ASCII.GetString(bs, mSGLength, (int) this.m_messageLength);
                }
                else
                {
                    this.m_messageContent = Encoding.Default.GetString(bs, mSGLength, (int) this.m_messageLength);
                }
            }
            else
            {
                this.m_messageContent = Encoding.BigEndianUnicode.GetString(bs, mSGLength, (int) this.m_messageLength);
            }
            mSGLength += (int) this.m_messageLength;
            Buffer.BlockCopy(bs, mSGLength, this.m_linkID, 0, 8);
        }

        public string LinkID
        {
            get
            {
                return (BitConvert.bytes2Uint(this.m_linkID, 0).ToString() + BitConvert.bytes2Uint(this.m_linkID, 4).ToString());
            }
        }

        public uint MessageCoding
        {
            get
            {
                return this.m_messageCoding;
            }
        }

        public string MessageContent
        {
            get
            {
                return this.m_messageContent;
            }
        }

        public uint MessageLength
        {
            get
            {
                return this.m_messageLength;
            }
        }

        public string SPNumber
        {
            get
            {
                return this.m_SpNumber;
            }
        }

        public int TP_pid
        {
            get
            {
                return this.m_tp_pid;
            }
        }

        public int TP_udhi
        {
            get
            {
                return this.m_tp_udhi;
            }
        }

        public string UserNumber
        {
            get
            {
                return this.m_UserNumber;
            }
        }
    }
}

