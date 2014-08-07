namespace KeywaySoft.Public.SGIP.Command
{
    using KeywaySoft.Public.SGIP.Base;
    using System;

    public class BindResp : BaseCommand
    {
        private int _result;

        public BindResp(byte[] seq) : base(0x80000001, seq)
        {
        }

        public BindResp(byte[] bs, int index) : base(bs, index)
        {
            if (BitConvert.bytes2Uint(bs, 0) == bs.Length)
            {
                this._result = bs[MSGHead.MSGLength];
            }
        }

        public override byte[] toBytes()
        {
            byte[] array = new byte[(MSGHead.MSGLength + 1) + 8];
            base._MSGHead.msgLength = (uint) array.Length;
            base._MSGHead.toBytes().CopyTo(array, 0);
            array[MSGHead.MSGLength] = (byte) this._result;
            return array;
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

