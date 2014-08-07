namespace KeywaySoft.Public.SGIP.Base
{
    using System;

    internal class ErrorMessageHelper
    {
        private ErrorMessageHelper()
        {
        }

        public static string GetMsg(ErrorCodes err)
        {
            return GetMsg((uint) err);
        }

        public static string GetMsg(uint nErrorCode)
        {
            switch (nErrorCode)
            {
                case 0:
                    return "无错误，命令正确接收";

                case 1:
                    return "非法登录，如登录名、口令出错、登录名与口令不符等。";

                case 2:
                    return "重复登录，如在同一TCP/IP连接中连续两次以上请求登录。";

                case 3:
                    return "连接过多，指单个节点要求同时建立的连接数过多。";

                case 4:
                    return "登录类型错，指bind命令中的logintype字段出错。";

                case 5:
                    return "参数格式错，指命令中参数值与参数类型不符或与协议规定的范围不符。";

                case 6:
                    return "非法手机号码，协议中所有手机号码字段出现非86130号码或手机号码前未加“86”时都应报错。";

                case 7:
                    return "消息ID错";

                case 8:
                    return "信息长度错";

                case 9:
                    return "非法序列号，包括序列号重复、序列号格式错误等";

                case 10:
                    return "非法操作GNS";

                case 11:
                    return "节点忙，指本节点存储队列满或其他原因，暂时不能提供服务的情况";

                case 0x15:
                    return "目的地址不可达，指路由表存在路由且消息路由正确但被路由的节点暂时不能提供服务的情况";

                case 0x16:
                    return "路由错，指路由表存在路由但消息路由出错的情况，如转错SMG等";

                case 0x17:
                    return "路由不存在，指消息路由的节点在路由表中不存在";

                case 0x18:
                    return "计费号码无效，鉴权不成功时反馈的错误信息";

                case 0x19:
                    return "用户不能通信（如不在服务区、未开机等情况）";

                case 0x1a:
                    return "手机内存不足";

                case 0x1b:
                    return "手机不支持短消息";

                case 0x1c:
                    return "手机接收短消息出现错误";

                case 0x1d:
                    return "不知道的用户";

                case 30:
                    return "不提供此功能";

                case 0x1f:
                    return "非法设备";

                case 0x20:
                    return "系统失败";

                case 0x21:
                    return "短信中心队列满";
            }
            return "其它其它错误码(待定义)";
        }
    }
}

