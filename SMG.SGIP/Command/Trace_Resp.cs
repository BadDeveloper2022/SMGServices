using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    public class Trace_Resp : BaseCommand
    {
        #region fields

        /// <summary>
        /// 被跟踪MT短消息经过的节点个数，当被跟踪短消息经过多个节点时，以下各个字段可重复 1字节
        /// </summary>
        public uint Count { get; set; }

        /// <summary>
        /// Trace命令在该节点是否成功接收。十六进制数字 1字节
        /// 0：接收成功
        /// 1：等待处理
        /// 其它：错误码
        /// </summary>
        public uint Result { get; set; }

        /// <summary>
        /// 节点编号  10字节
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// 被跟踪的短消息到达该节点时刻，格式为“yymmddhhmmss”  16字节
        /// </summary>
        public string ReceiveTime { get; set; }

        /// <summary>
        /// 该节点发出被跟踪的短消息时刻，格式为“yymmddhhmmss”  16字节
        /// </summary>
        public string SendTime { get; set; }

        #endregion

        public Trace_Resp()
        {
            base.Command = Commands.Trace_Resp;
        }

        public Trace_Resp(byte[] bytes)
            : base(bytes)
        {
            int offset = HEADER_LENGTH;
            this.Count = bytes[offset];
            offset++;
            this.Result = bytes[offset];
            offset++;
            this.NodeId = GetString(bytes, offset, 10);
            offset += 10;
            this.ReceiveTime = GetString(bytes, offset, 16);
            offset += 10;
            this.SendTime = GetString(bytes, offset, 16);
        }

        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[HEADER_LENGTH + 12 + 21 + 8];
            base.TotalMessageLength = (uint)bytes.Length;

            //消息头
            base.Headers.CopyTo(bytes, 0);
            //消息体
            int offset = HEADER_LENGTH;
            bytes[offset] = (byte)Count;
            bytes[offset] = (byte)Result;
            byte[] nibts = GetBytes(NodeId);
            Array.Copy(nibts, 0, bytes, offset, nibts.Length);
            offset += 10;
            byte[] rtbts = GetBytes(ReceiveTime);
            Array.Copy(rtbts, 0, bytes, offset, rtbts.Length);
            offset += 16;
            byte[] stbts = GetBytes(ReceiveTime);
            Array.Copy(stbts, 0, bytes, offset, stbts.Length);

            return bytes;
        }

    }
}
