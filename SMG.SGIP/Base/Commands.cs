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
        public const uint CheckUser = 0x10;
        public const uint CheckUser_Resp = 0x80000010;
        public const uint UserRpt = 0x11;
        public const uint UserRpt_Resp = 0x80000011;
        public const uint Trace = 0x1000;
        public const uint Trace_Resp = 0x80001000;


        public static string GetString(uint cmd)
        {
            string desc = "unkown";
            switch (cmd)
            {
                case Bind:
                    desc = "Bind";
                    break;
                case Bind_Resp:
                    desc = "Bind_Resp";
                    break;
                case UnBind_Resp:
                    desc = "UnBind_Resp";
                    break;
                case Submit:
                    desc = "Submit";
                    break;
                case Submit_Resp:
                    desc = "Submit_Resp";
                    break;
                case Deliver:
                    desc = "Deliver";
                    break;
                case Deliver_Resp:
                    desc = "Deliver_Resp";
                    break;
                case Report:
                    desc = "Report";
                    break;
                case Report_Resp:
                    desc = "Report_Resp";
                    break;
                case CheckUser:
                    desc = "CheckUser";
                    break;
                case CheckUser_Resp:
                    desc = "CheckUser_Resp";
                    break;
                case UserRpt:
                    desc = "UserRpt";
                    break;
                case UserRpt_Resp:
                    desc = "UserRpt_Resp";
                    break;
                case Trace:
                    desc = "Trace";
                    break;
                case Trace_Resp:
                    desc = "Trace_Resp";
                    break;
                default:
                    break;
            }

            return desc;
        }
    }
}
