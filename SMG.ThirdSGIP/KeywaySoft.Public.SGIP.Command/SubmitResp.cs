namespace KeywaySoft.Public.SGIP.Command
{
    using KeywaySoft.Public.SGIP.Base;
    using System;

    public class SubmitResp : BaseCommand
    {
        private int _result;

        public SubmitResp(byte[] seq) : base(0x80000003, seq)
        {
        }

        public SubmitResp(byte[] bs, int index) : base(bs, index)
        {
            this._result = bs[MSGHead.MSGLength];
        }

        public int Result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }
    }
}

