namespace KeywaySoft.Public.SGIP.Base
{
    using System;

    public enum MSGCommandId : uint
    {
        SGIP_BIND = 1,
        SGIP_BIND_RES = 0x80000001,
        SGIP_DELIVER = 4,
        SGIP_DELIVER_RESP = 0x80000004,
        SGIP_REPORT = 5,
        SGIP_REPORT_RESP = 0x80000005,
        SGIP_SUBMIT = 3,
        SGIP_SUBMIT_RESP = 0x80000003,
        SGIP_TRACE = 0x1000,
        SGIP_TRACE_RESP = 0x80001000,
        SGIP_UNBIND = 2,
        SGIP_UNBIND_RESP = 0x80000002,
        UserRpt = 0x11,
        UserRpt_Resp = 0x80000011
    }
}

