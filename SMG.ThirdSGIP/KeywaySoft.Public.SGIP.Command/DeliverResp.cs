namespace KeywaySoft.Public.SGIP.Command
{
    using KeywaySoft.Public.SGIP.Base;
    using System;

    public class DeliverResp : BaseCommand
    {
        private string _reserve;
        private int _result;

        public DeliverResp(byte[] seq) : base(0x80000004, seq)
        {
        }

        public DeliverResp(byte[] bs, int index) : base(bs, index)
        {
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

