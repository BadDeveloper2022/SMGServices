using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Base
{
    public class BaseCommand
    {
        public const int HEADER_LENGTH = 20;

        #region header fields

        /// <summary>
        /// 消息的总长度(字节) 4字节
        /// </summary>
        public uint MessageLength { get; protected set; }

        /// <summary>
        /// 命令ID 4字节
        /// </summary>
        public uint Command { get; protected set; }

        /// <summary>
        /// 序列号 12字节 , 响应的序列号和接受到的请求序列号一致
        /// </summary>
        public byte[] SequenceNumber { get; set; }

        /// <summary>
        /// 节点号 4字节
        /// </summary>
        public uint NodeNumber { get; private set; }

        /// <summary>
        /// 时间 4字节
        /// </summary>
        public uint DateTime { get; private set; }

        /// <summary>
        /// 源序列号 4字节
        /// </summary>
        public uint OrdinalNumber { get; private set; }

        /// <summary>
        /// 消息头 20字节
        /// </summary>
        public byte[] Headers
        {
            get
            {
                byte[] headers = new byte[HEADER_LENGTH];
                NetBitConverter.GetBytes(this.MessageLength).CopyTo(headers, 0);
                Array.Copy(NetBitConverter.GetBytes(this.Command), 0, headers, 4, 4);
                Array.Copy(this.SequenceNumber, 0, headers, 8, 12);

                return headers;
            }
        }

        #endregion

        #region public fields

        /// <summary>
        /// 保留扩展用 8字节
        /// </summary>
        public string Reserve { get; set; }

        #endregion

        public BaseCommand()
        {
        }

        public BaseCommand(byte[] bytes)
        {
            this.MessageLength = NetBitConverter.ToUInt32(bytes, 0);
            if (bytes.Length != this.MessageLength) throw new Exception("消息接收不完整");

            this.Command = NetBitConverter.ToUInt32(bytes, 4);
            this.SequenceNumber = new byte[12];
            Array.Copy(bytes, 8, this.SequenceNumber, 0, 12);
            this.NodeNumber = NetBitConverter.ToUInt32(this.SequenceNumber, 0);
            this.DateTime = NetBitConverter.ToUInt32(this.SequenceNumber, 4);
            this.OrdinalNumber = NetBitConverter.ToUInt32(this.SequenceNumber, 8);
        }

        public virtual byte[] GetBytes()
        {
            return this.Headers;
        }

        public byte[] GetBytes(string value)
        {
            return Encoding.ASCII.GetBytes(value);
        }

        public string GetString(byte[] bytes, int index, int count)
        {
            IList<byte> dst = new List<byte>();
            for (int i = index; i < index + count; i++)
            {
                if (bytes[i] == 0) break;
                dst.Add(bytes[i]);
            }

            return Encoding.ASCII.GetString(dst.ToArray());
        }
    }
}
