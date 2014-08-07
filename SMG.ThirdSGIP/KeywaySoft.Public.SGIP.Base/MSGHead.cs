namespace KeywaySoft.Public.SGIP.Base
{
    using System;

    public class MSGHead
    {
        private uint m_commandID;
        private uint m_msgLength;
        private byte[] m_Sequencenumber;

        public MSGHead(byte[] bs)
        {
            this.m_Sequencenumber = new byte[12];
            int index = 0;
            this.m_msgLength = BitConvert.bytes2Uint(bs, index);
            index += 4;
            this.m_commandID = BitConvert.bytes2Uint(bs, index);
            index += 4;
            Buffer.BlockCopy(bs, index, this.m_Sequencenumber, 0, 12);
        }

        public MSGHead(uint msgFormat)
        {
            this.m_Sequencenumber = new byte[12];
            this.m_commandID = msgFormat;
        }

        public byte[] toBytes()
        {
            byte[] array = new byte[MSGLength];
            byte[] buffer2 = BitConvert.uint2Bytes(this.m_msgLength);
            byte[] src = BitConvert.uint2Bytes(this.m_commandID);
            int dstOffset = 0;
            buffer2.CopyTo(array, 0);
            dstOffset += 4;
            Buffer.BlockCopy(src, 0, array, dstOffset, 4);
            dstOffset += 4;
            Buffer.BlockCopy(this.m_Sequencenumber, 0, array, dstOffset, 12);
            return array;
        }

        public uint commandID
        {
            get
            {
                return this.m_commandID;
            }
        }

        public uint DateTime
        {
            get
            {
                return BitConvert.bytes2Uint(this.m_Sequencenumber, 4);
            }
        }

        public uint msgLength
        {
            get
            {
                return this.m_msgLength;
            }
            set
            {
                this.m_msgLength = value;
            }
        }

        public static uint MSGLength
        {
            get
            {
                return 20;
            }
        }

        public uint NodeNumber
        {
            get
            {
                return BitConvert.bytes2Uint(this.m_Sequencenumber, 0);
            }
        }

        public uint OrdinalNumber
        {
            get
            {
                return BitConvert.bytes2Uint(this.m_Sequencenumber, 8);
            }
        }

        public byte[] SequenceNumber
        {
            get
            {
                return this.m_Sequencenumber;
            }
            set
            {
                this.m_Sequencenumber = value;
            }
        }
    }
}

