using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Base
{
    public class FeeTypes
    {
        /// <summary>
        /// “短消息类型”为“发送”，对“计费用户号码”不计信息费，此类话单仅用于核减SP对称的信道费
        /// </summary>
        public const uint SendFree = 0;

        /// <summary>
        /// 对“计费用户号码”免费
        /// </summary>
        public const uint Free = 1;

        /// <summary>
        /// 对“计费用户号码”按条计信息费
        /// </summary>
        public const uint Item = 2;

        /// <summary>
        /// 对“计费用户号码”按包月收取信息费
        /// </summary>
        public const uint Month = 3;

        /// <summary>
        /// 对“计费用户号码”的收费是由SP实现	
        /// </summary>
        public const uint SP = 4;

    }
}
