namespace KeywaySoft.Public.SGIP.Command
{
    using KeywaySoft.Public.SGIP.Base;
    using System;

    public class Unbind : BaseCommand
    {
        public Unbind(byte[] seq) : base(2, seq)
        {
        }

        public Unbind(byte[] bs, int index) : base(bs, index)
        {
        }
    }
}

