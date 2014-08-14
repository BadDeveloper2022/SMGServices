using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Base
{
    public class ReportStatus
    {
        /// <summary>
        /// 发送成功 
        /// 短消息状态：DELIVERED
        /// </summary>
        public const uint Success = 0;

        /// <summary>
        /// 等待发送
        /// 短消息状态：ENROUTE，ACCEPTED
        /// </summary>
        public const uint Wait = 1;

        /// <summary>
        /// 发送失败
        /// 短消息状态：EXPIRED，DELETED，UNDELIVERABLE，UNKNOWN，REJECTED
        /// </summary>
        public const uint Fail = 2;
    }
}
