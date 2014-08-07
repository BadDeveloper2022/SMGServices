namespace KeywaySoft.Public.SGIP.Command
{
    using KeywaySoft.Public.SGIP.Base;
    using System;

    public class UnbindResp : BaseCommand
    {
        public UnbindResp(byte[] seq) : base(0x80000002, seq)
        {
        }

        public UnbindResp(byte[] bs, int index) : base(bs, index)
        {
        }
    }
}

