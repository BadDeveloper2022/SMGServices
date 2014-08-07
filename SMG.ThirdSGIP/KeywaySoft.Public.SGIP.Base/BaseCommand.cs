namespace KeywaySoft.Public.SGIP.Base
{
    using System;

    public class BaseCommand
    {
        protected MSGHead _MSGHead;

        public BaseCommand(uint msgFormat, byte[] sequenceNumber)
        {
            this._MSGHead = new MSGHead(msgFormat);
            this._MSGHead.msgLength = MSGHead.MSGLength;
            this._MSGHead.SequenceNumber = sequenceNumber;
        }

        public BaseCommand(byte[] bs, int msgCount)
        {
            byte[] destinationArray = new byte[msgCount];
            Array.Copy(bs, destinationArray, msgCount);
            this._MSGHead = new MSGHead(destinationArray);
        }

        public virtual byte[] toBytes()
        {
            return this._MSGHead.toBytes();
        }

        public uint DateTime
        {
            get
            {
                return this._MSGHead.DateTime;
            }
        }

        public uint NodeNumber
        {
            get
            {
                return this._MSGHead.NodeNumber;
            }
        }

        public uint OrdinalNumber
        {
            get
            {
                return this._MSGHead.OrdinalNumber;
            }
        }

        public byte[] SequenceNumber
        {
            get
            {
                return this._MSGHead.SequenceNumber;
            }
        }
    }
}

