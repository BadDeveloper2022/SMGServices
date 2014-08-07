namespace KeywaySoft.Public.SGIP.Command
{
    using KeywaySoft.Public.SGIP.Base;
    using System;
    using System.Text;

    public class Report : BaseCommand
    {
        private int _errCode;
        private int _reportType;
        private string _reserve;
        private int _stste;
        private string _userNumber;
        private byte[] m_OldSubmitSequenceNumber;

        public Report(byte[] seq) : base(5, seq)
        {
        }

        public Report(byte[] bs, int msgCount) : base(bs, msgCount)
        {
            int mSGLength = (int) MSGHead.MSGLength;
            Buffer.BlockCopy(bs, mSGLength, this.m_OldSubmitSequenceNumber, 0, 12);
            mSGLength += 12;
            this._reportType = bs[mSGLength++];
            this._userNumber = Encoding.ASCII.GetString(bs, mSGLength, 0x15);
            mSGLength += 0x15;
            this._stste = bs[mSGLength++];
            this._reserve = Encoding.ASCII.GetString(bs, mSGLength, 8);
        }

        public int ErrorCode
        {
            get
            {
                return this._errCode;
            }
        }

        public string OldDateTimeNumber
        {
            get
            {
                return BitConvert.bytes2Uint(this.m_OldSubmitSequenceNumber, 4).ToString();
            }
        }

        public uint OldNodeNumber
        {
            get
            {
                return BitConvert.bytes2Uint(this.m_OldSubmitSequenceNumber, 0);
            }
        }

        public string OldOrdinalNumber
        {
            get
            {
                return BitConvert.bytes2Uint(this.m_OldSubmitSequenceNumber, 8).ToString();
            }
        }

        public byte[] OldSequenceNumber
        {
            get
            {
                return this.m_OldSubmitSequenceNumber;
            }
        }

        public int ReportType
        {
            get
            {
                return this._reportType;
            }
        }

        public string Reserve
        {
            get
            {
                return this._reserve;
            }
        }

        public int State
        {
            get
            {
                return this._stste;
            }
        }

        public string UserNumber
        {
            get
            {
                return this._userNumber;
            }
        }
    }
}

