using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public uint TotalMessageLength { get; protected set; }

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
                GetBytes(this.TotalMessageLength).CopyTo(headers, 0);
                Array.Copy(GetBytes(this.Command), 0, headers, 4, 4);
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
            this.TotalMessageLength = ToUInt32(bytes, 0, 4);
            if (bytes.Length != this.TotalMessageLength) throw new Exception("消息接收不完整");

            this.Command = ToUInt32(bytes, 4, 4);
            this.SequenceNumber = new byte[12];
            Array.Copy(bytes, 8, this.SequenceNumber, 0, 12);
            this.NodeNumber = ToUInt32(this.SequenceNumber, 0, 4);
            this.DateTime = ToUInt32(this.SequenceNumber, 4, 4);
            this.OrdinalNumber = ToUInt32(this.SequenceNumber, 8, 4);
        }

        public virtual byte[] GetBytes()
        {
            this.TotalMessageLength = HEADER_LENGTH;
            return this.Headers;
        }

        public byte[] GetBytes(uint value)
        {
            byte[] dst = BitConverter.GetBytes(value);
            dst = dst.Reverse().ToArray();

            return dst;
        }

        public byte[] GetBytes(string value)
        {
            return GetBytes(Encoding.ASCII, value);
        }

        public byte[] GetBytes(Encoding charset, string value)
        {
            return charset.GetBytes(value);
        }

        public uint ToUInt32(byte[] values, int index, int length)
        {
            byte[] dst = new byte[4];
            Array.Copy(values, index, dst, 0, length);
            dst = dst.Reverse().ToArray();

            return BitConverter.ToUInt32(dst, 0);
        }

        public string GetString(byte[] bytes, int index, int count)
        {
            return GetString(Encoding.ASCII, bytes, index, count);
        }

        public string GetString(Encoding charset, byte[] bytes, int index, int count)
        {
            IList<byte> dst = new List<byte>();
            for (int i = index; i < index + count; i++)
            {
                if (bytes[i] == 0) break;
                dst.Add(bytes[i]);
            }

            return charset.GetString(dst.ToArray());
        }

        public override string ToString()
        {
            Type t = this.GetType();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(t.Name + " fields info ： ");

            foreach (var field in t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
            {
                var v = field.GetValue(this);
                sb.AppendLine("\t" + field.Name.Substring(0, field.Name.LastIndexOf(">") + 1)
                    + " = " + (v != null ? v.ToString() : "null"));
            }

            return sb.ToString();
        }
    }
}
