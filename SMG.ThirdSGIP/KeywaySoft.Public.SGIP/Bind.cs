namespace KeywaySoft.Public.SGIP
{
    using KeywaySoft.Public.SGIP.Base;
    using System;
    using System.Text;

    public class Bind : BaseCommand
    {
        private string _LoginName;
        private string _LoginPassword;
        private int _LoginType;

        public Bind(byte[] bs, int msgCount) : base(bs, msgCount)
        {
            int mSGLength = (int) MSGHead.MSGLength;
            this._LoginType = bs[mSGLength++];
            this._LoginName = Encoding.ASCII.GetString(bs, mSGLength, 0x10).Replace("\0", "");
            mSGLength += 0x10;
            this._LoginPassword = Encoding.ASCII.GetString(bs, mSGLength, 0x10).Replace("\0", "");
        }

        public Bind(int loginType, string loginName, string loginPassword) : base(1, Logic.GetNextSequence())
        {
            this._LoginName = loginName;
            this._LoginType = loginType;
            this._LoginPassword = loginPassword;
        }

        public override byte[] toBytes()
        {
            byte[] array = new byte[(((MSGHead.MSGLength + 1) + 0x10) + 0x10) + 8];
            base._MSGHead.msgLength = (uint) array.Length;
            base._MSGHead.toBytes().CopyTo(array, 0);
            int mSGLength = (int) MSGHead.MSGLength;
            array[mSGLength++] = (byte) this._LoginType;
            byte[] bytes = Encoding.ASCII.GetBytes(this._LoginName);
            Buffer.BlockCopy(bytes, 0, array, mSGLength, bytes.Length);
            mSGLength += 0x10;
            byte[] src = Encoding.ASCII.GetBytes(this._LoginPassword);
            Buffer.BlockCopy(src, 0, array, mSGLength, src.Length);
            return array;
        }

        public string LoginName
        {
            get
            {
                return this._LoginName;
            }
        }

        public string LoginPassword
        {
            get
            {
                return this._LoginPassword;
            }
        }

        public int LoginType
        {
            get
            {
                return this._LoginType;
            }
        }
    }
}

