using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Base
{
    /// <summary>
    /// 错误码1-20所指错误一般在各类命令的应答中用到，21-32所指错误一般在report命令中用到
    /// </summary>
    public class CommandError
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const uint Success = 0;
        
        /// <summary>
        /// 非法登陆
        /// </summary>
        public const uint InvalidLogin = 1;
        
        /// <summary>
        /// 重复登陆
        /// </summary>
        public const uint RepeatLogin = 2;

        /// <summary>
        /// 连接过多
        /// </summary>
        public const uint MaxConnections = 3;

        /// <summary>
        /// 登陆类型错误
        /// </summary>
        public const uint InvalidLoginType = 4;

        /// <summary>
        /// 参数格式错误
        /// </summary>
        public const uint InvalidParameters = 5;

        /// <summary>
        /// 非法手机号码
        /// </summary>
        public const uint InvalidUserNumber = 6;

        /// <summary>
        /// 消息ID错误
        /// </summary>
        public const uint InvalidCommandId = 7;

        /// <summary>
        /// 消息长度错误
        /// </summary>
        public const uint InvalidMessageLength = 8;

        /// <summary>
        /// 非法序列号
        /// </summary>
        public const uint InvalidSequenceNumber = 9;

        /// <summary>
        /// 非法操作GNS
        /// </summary>
        public const uint InvalidGNSOperation = 10;

        /// <summary>
        /// 节点忙
        /// </summary>
        public const uint NodeBusy = 11;

        /// <summary>
        /// 业务代码未分配
        /// </summary>
        public const uint ServiceCodeUnallocated = 13;

        /// <summary>
        /// 业务资费类型错误
        /// </summary>
        public const uint ServiceFeeTypeInvalid = 14;

        /// <summary>
        /// LINKID不匹配
        /// </summary>
        public const uint LinkIDNotMatch = 17;

        /// <summary>
        /// 用户未订购 
        /// </summary>
        public const uint UserNotOrder = 18;

        /// <summary>
        /// 下发用户数不为1
        /// </summary>
        public const uint InvalidUserCount = 19;

        /// <summary>
        /// 目的地址不可达
        /// </summary>
        public const uint DestinationUnreachable = 21;

        /// <summary>
        /// 路由错误
        /// </summary>
        public const uint InvalidRouter = 22;

        /// <summary>
        /// 路由不存在
        /// </summary>
        public const uint RouterNotExists = 23;

        /// <summary>
        /// 计费号码无效
        /// </summary>
        public const uint InvalidChargeNumber = 24;

        /// <summary>
        /// 用户不能通信
        /// </summary>
        public const uint UserUnCommunication = 25;

        /// <summary>
        /// 手机内存不足
        /// </summary>
        public const uint PhoneMemoryPoor = 26;

        /// <summary>
        /// 手机不支持短消息
        /// </summary>
        public const uint PhoneSMSNUnSupport = 27;

        /// <summary>
        /// 手机接收短消息出现错误
        /// </summary>
        public const uint PhoneReceiveSMSError = 28;

        /// <summary>
        /// 不知道的用户
        /// </summary>
        public const uint UnknownUser = 29;

        /// <summary>
        /// 不提供此功能
        /// </summary>
        public const uint NotSupportFunction = 30;

        /// <summary>
        /// 非法设备
        /// </summary>
        public const uint InvalidDevice = 31;

        /// <summary>
        /// 该包格式错误
        /// </summary>
        public const uint InvalidPackageFormat = 32;

        public static string GetMessage(uint code)
        {
            string message = "";

            switch (code)
            {
                case Success:
                    message = "成功";
                    break;
                case InvalidLogin:
                    message = "非法登陆";
                    break;
                case RepeatLogin:
                    message = "重复登陆";
                    break;
                case MaxConnections:
                    message = "连接过多";
                    break;
                case InvalidLoginType:
                    message = "登陆类型错误";
                    break;
                case InvalidParameters:
                    message = "参数格式错误";
                    break;
                case InvalidUserNumber:
                    message = "非法手机号码";
                    break;
                case InvalidCommandId:
                    message = "消息ID错误";
                    break;
                case InvalidMessageLength:
                    message = "消息长度错误";
                    break;
                case InvalidSequenceNumber:
                    message = "非法序列号";
                    break;
                case InvalidGNSOperation:
                    message = "非法操作GNS";
                    break;
                case NodeBusy:
                    message = "节点忙";
                    break;
                case ServiceCodeUnallocated:
                    message = "业务代码未分配";
                    break;
                case ServiceFeeTypeInvalid:
                    message = "业务资费类型错误";
                    break;
                case LinkIDNotMatch:
                    message = "LINKID不匹配";
                    break;
                case UserNotOrder:
                    message = "用户未订购";
                    break;
                case InvalidUserCount:
                    message = "下发用户数不为1";
                    break;
                case DestinationUnreachable:
                    message = "目的地址不可达";
                    break;
                case InvalidRouter:
                    message = "路由错误";
                    break;
                case RouterNotExists:
                    message = "路由不存在";
                    break;
                case InvalidChargeNumber:
                    message = "计费号码无效";
                    break;
                case UserUnCommunication:
                    message = "用户不能通信";
                    break;
                case PhoneMemoryPoor:
                    message = "手机内存不足";
                    break;
                case PhoneSMSNUnSupport:
                    message = "手机不支持短消息";
                    break;
                case PhoneReceiveSMSError:
                    message = "手机接收短消息出现错误";
                    break;
                case UnknownUser:
                    message = "不知道的用户";
                    break;
                case NotSupportFunction:
                    message = "不提供此功能";
                    break;
                case InvalidDevice:
                    message = "非法设备";
                    break;
                case InvalidPackageFormat:
                    message = "该包格式错误";
                    break;
                default:
                    message = "其它其它错误码(待定义)";
                    break;
            }

            return message;
        }
    }
}
