namespace KeywaySoft.Public.SGIP.Command
{
    using KeywaySoft.Public.SGIP.Base;
    using System;
    using System.Text;

    public class ReportResp : BaseCommand
    {
        private string _reserve;
        private int _result;

        public ReportResp(byte[] seq) : base(0x80000005, seq)
        {
        }

        public ReportResp(byte[] bs, int index) : base(bs, index)
        {
            int mSGLength = (int) MSGHead.MSGLength;
            this._result = bs[mSGLength++];
            this._reserve = Encoding.ASCII.GetString(bs, mSGLength, 8);
        }

        public string Reserve
        {
            get
            {
                return this.Reserve;
            }
        }

        public int Result
        {
            get
            {
                return this._result;
            }
        }
    }
}

