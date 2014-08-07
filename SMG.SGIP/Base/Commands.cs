using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Base
{
    public class Commands
    {
        public const uint Bind = 1;
        public const uint Bind_Resp = 0x80000001;
        public const uint UnBind = 2;
        public const uint UnBind_Resp = 0x80000002;
        public const uint Submit = 3;
        public const uint Submit_Resp = 0x80000003;
        public const uint Deliver = 4;
        public const uint Deliver_Resp = 0x80000004;
        public const uint Report = 5;
        public const uint Report_Resp = 0x80000005;
        public const uint Trace = 0x1000;
        public const uint Trace_Resp = 0x80001000;
        public const uint UserRpt = 0x11;
        public const uint UserRpt_Resp = 0x80000011;
    }
}
