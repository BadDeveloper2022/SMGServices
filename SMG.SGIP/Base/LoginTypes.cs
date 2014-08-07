using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Base
{
    public class LoginTypes
    {
        /// <summary>
        /// 其他，保留
        /// </summary>
        public const uint Unknown = 0;

        /// <summary>
        /// SP向SMG建立的连接，用于发送命令
        /// </summary>
        public const uint SPToSMG = 1;

        /// <summary>
        /// SMG向SP建立的连接，用于发送命令
        /// </summary>
        public const uint SMGToSP = 2;

        /// <summary>
        /// SMG之间建立的连接，用于转发命令
        /// </summary>
        public const uint SMGToSMG = 3;

        /// <summary>
        /// SMG向GNS建立的连接，用于路由表的检索和维护
        /// </summary>
        public const uint SMGToGNS = 4;

        /// <summary>
        /// GNS向SMG建立的连接，用于路由表的更新
        /// </summary>
        public const uint GNSToSMG = 5;

        /// <summary>
        /// 主备GNS之间建立的连接，用于主备路由表的一致性
        /// </summary>
        public const uint GNSToGNS = 6;

        /// <summary>
        /// SP与SMG以及SMG之间建立的测试连接，用于跟踪测试
        /// </summary>
        public const uint Test = 11;

    }
}
