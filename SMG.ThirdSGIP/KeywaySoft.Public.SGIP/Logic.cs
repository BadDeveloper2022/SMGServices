namespace KeywaySoft.Public.SGIP
{
    using KeywaySoft.Public.SGIP.Base;
    using KeywaySoft.Public.SGIP.Command;
    using KeywaySoft.Public.SGIP.DataBase;
    using KeywaySoft.Public.SGIP.SGIPSocket;
    using KeywaySoft.Public.SGIP.SGIPThread;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Diagnostics;

    public class Logic
    {
        public string DB_IP = "";
        public string DB_NAME = "";
        public string DB_PASSWORD = "";
        public int DB_PORT = 0x599;
        public string DB_USERNAME = "";
        private DBClass m_DB;
        private BaseThread m_MonitoringClientThread;
        private int m_MonitoringTime = 0;
        private static uint m_NodeNumber = 0x4d2;
        private QueueList m_OutSeqQueue = new QueueList();
        private BaseThread m_ScanDataThread;
        private BaseThread m_SendSMSThread;
        private static uint m_Sequence = 0;
        private TCPSocketClient m_TcpSocketClient;
        private TcpSocketServer m_TcpSocketServer;
        private QueueList m_WaitSeqQueue = new QueueList();
        private const uint MAX_SEND_COUNT = 0x20;
        private const uint MaxDifferSendAndRecvTime = 30;
        private const uint MaxSpareTime = 60;
        public static int RecvMOCount = 0;
        public static int SendMTCount = 0;
        public string SMG_AREACODE = "";
        public string SMG_IP = "";
        public string SMG_PASS = "";
        public int SMG_PORT = 0x2262;
        public string SMG_SPCODE = "";
        public string SMG_USER = "";
        public string SP_IP = "";
        public string SP_PASS = "";
        public int SP_PORT = 0x2261;
        public string SP_USER = "";
        public int SmsFlagSolution = -1;

        public event AddTextDelegate AddTextEvent;

        public event SystemDelegate StartEvent;

        public event SystemDelegate StopEvent;
        public event SmsResearchDelegate SmsResearchEvent;

        private void CheckReSend()
        {
            if (this.m_MonitoringTime > 30)
            {
                SortedList<uint, QueueItem> list = this.m_OutSeqQueue.List;
                for (int i = 0; i < list.Count; i++)
                {
                    QueueItem item = list.Values[i];
                    TimeSpan span = (TimeSpan) (DateTime.Now - item.inQueueTime);
                    if (span.TotalSeconds > 30.0)
                    {
                        Logic logic;
                        if (item.FailedCount > 3)
                        {
                            lock ((logic = this))
                            {
                                this.m_OutSeqQueue.Remove(item.Sequence);
                            }
                            this.m_DB.ExeSQL("exec [sp_sms_Recv_SubmitResp] " + item.SqlID + ",-3");
                        }
                        else
                        {
                            item.FailedCount++;
                            lock ((logic = this))
                            {
                                this.m_WaitSeqQueue.Add(item.Sequence, item);
                                this.m_OutSeqQueue.Remove(item.Sequence);
                            }
                        }
                    }
                }
            }
        }

        private bool ClientLoginEvent(byte[] bs)
        {
            if ((BitConvert.bytes2Uint(bs, 0) == bs.Length) && (BitConvert.bytes2Uint(bs, 4) == 0x80000001))
            {
                BindResp resp = new BindResp(bs, bs.Length);
                this.AddTextEvent(this.GetErrorCodes(resp.Result));
                if (resp.Result == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void ClientRecvEvent(byte[] msg, int recvCount, Socket client)
        {
            string sql = "";
            switch (BitConvert.bytes2Uint(msg, 4))
            {
                case 0x80000002:
                    this.m_TcpSocketClient.CloseSocket();
                    this.AddTextEvent("已经和SMG断开连接");
                    break;

                case 0x80000003:
                {
                    SubmitResp resp = new SubmitResp(msg, recvCount);
                    QueueItem item = this.m_OutSeqQueue[resp.OrdinalNumber];
                    if (item != null)
                    {
                        item.msgState = 3;
                        this.m_OutSeqQueue.Remove(item.Sequence);
                        sql = string.Format("exec sp_sms_Recv_SubmitResp {0},{1}", item.SqlID, resp.Result);
                        this.m_DB.ExeSQL(sql);
                        this.AddTextEvent("已经和SMG断开连接");
                    }                    
                    break;
                }
                case 2:
                {
                    this.AddTextEvent("接收到SMG断开连接请求");
                    Unbind unbind = new Unbind(msg, recvCount);
                    this.m_TcpSocketClient.SendData(new UnbindResp(unbind.SequenceNumber).toBytes());
                    this.AddTextEvent("已经和SMG断开连接");
                    break;
                }
            }
        }

        private static uint GetDateTime()
        {
            DateTime now = DateTime.Now;
            return uint.Parse(now.Month.ToString().PadLeft(2, '0') + now.Day.ToString().PadLeft(2, '0') + now.Hour.ToString().PadLeft(2, '0') + now.Minute.ToString().PadLeft(2, '0') + now.Second.ToString().PadLeft(2, '0'));
        }

        public string GetErrorCodes(int errCode)
        {
            switch (errCode)
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
                    return "目的地址不可达，指路由表存在路由且消息路由正确但被路由的节点暂时不能提供服务的情况。";

                case 0x16:
                    return "路由错，指路由表存在路由但消息路由出错的情况，如转错SMG等。";

                case 0x17:
                    return "路由不存在，指消息路由的节点在路由表中不存在。";

                case 0x18:
                    return "计费号码无效，鉴权不成功时反馈的错误信息";

                case 0x19:
                    return "用户不能通信（如不在服务区、未开机等情况）";

                case 0x1a:
                    return "手机内存不足";

                case 0x1b:
                    return "手机不支持短消息。";

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
            return "未知错误";
        }

        private static uint getNextOrdinal()
        {
            try
            {
                m_Sequence++;
            }
            catch (OverflowException)
            {
                m_Sequence = uint.MaxValue;
            }
            return m_Sequence;
        }

        public static byte[] GetNextSequence()
        {
            byte[] array = new byte[12];
            BitConvert.uint2Bytes(m_NodeNumber).CopyTo(array, 0);
            Array.Copy(BitConvert.uint2Bytes(GetDateTime()), 0, array, 4, 4);
            Array.Copy(BitConvert.uint2Bytes(getNextOrdinal()), 0, array, 8, 4);
            return array;
        }

        public void Initialization()
        {
            this.m_MonitoringTime = 0;
            RecvMOCount = 0;
            SendMTCount = 0;
            this.m_OutSeqQueue = new QueueList();
            this.m_WaitSeqQueue = new QueueList();
            this.m_DB = new DBClass(this.DB_IP, this.DB_PORT, this.DB_NAME, this.DB_USERNAME, this.DB_PASSWORD);
            this.m_MonitoringClientThread = new BaseThread(0x3e8);
            this.m_MonitoringClientThread.ThreadEvent += new ThreadHandle(this.MonitoringClientEvent);
            this.m_ScanDataThread = new BaseThread(0x3e8);
            this.m_ScanDataThread.ThreadEvent += new ThreadHandle(this.ScanDataEvent);
            this.m_SendSMSThread = new BaseThread(0x3e8);
            this.m_SendSMSThread.ThreadEvent += new ThreadHandle(this.SendSMSEvent);
            this.m_TcpSocketServer = new TcpSocketServer(this.SP_IP, this.SP_PORT);
            this.m_TcpSocketServer.SPName = this.SP_USER;
            this.m_TcpSocketServer.SPPassword = this.SP_PASS;
            this.m_TcpSocketServer.RecvDataEvent += new NetEventDelegate(this.ServerRecvEvent);
            this.m_TcpSocketClient = new TCPSocketClient(this.SMG_IP, this.SMG_PORT);
            this.m_TcpSocketClient.RecvDataEvent += new NetEventDelegate(this.ClientRecvEvent);
            this.m_TcpSocketClient.LoginEvent += new LoginDelegate(this.ClientLoginEvent);
            this.m_TcpSocketClient.UserName = this.SMG_USER;
            this.m_TcpSocketClient.Password = this.SMG_PASS;
            this.m_TcpSocketClient.LoginType = 1;
            try
            {
                m_NodeNumber = uint.Parse(this.SMG_SPCODE);
            }
            catch
            {
                m_NodeNumber = 0x3e8;
            }
        }

        public void InQueueEvent(object id, object SPNumber, object ChargeNumber, object UserCount, object UserNumber, object CorpId, object ServiceType, object FeeType, object FeeValue, object GivenValue, object AgentFlag, object MorelatetoMTFlag, object Priority, object ExpireTime, object ScheduleTime, object ReportFlag, object TP_pid, object TP_udhi, object MessageCoding, object MessageType, object MessageContent, object linkid)
        {
            try
            {
                Submit submit = new Submit(GetNextSequence());
                submit.SPNumber = SPNumber.ToString();
                submit.ChargeNumber = ChargeNumber.ToString();
                submit.UserCount = (int) UserCount;
                submit.UserNumber = UserNumber.ToString();
                submit.CorpId = CorpId.ToString();
                submit.ServiceType = ServiceType.ToString();
                submit.FeeType = (int) FeeType;
                submit.FeeValue = FeeValue.ToString();
                submit.GivenValue = GivenValue.ToString();
                submit.AgentFlag = (int) AgentFlag;
                submit.MorelatetoMTFlag = (int) MorelatetoMTFlag;
                submit.Priority = (int) Priority;
                submit.ExpireTime = ExpireTime.ToString();
                submit.ScheduleTime = ScheduleTime.ToString();
                submit.ReportFlag = (int) ReportFlag;
                submit.TP_pid = (int) TP_pid;
                submit.TP_udhi = (int) TP_udhi;
                if (this.IsIncludeGB(MessageContent as string))
                {
                    submit.MessageCoding = 15;
                }
                else
                {
                    submit.MessageCoding = Convert.ToUInt32(MessageCoding); //(uint) MessageCoding;
                }
                submit.MessageContent = MessageContent.ToString();
                submit.LinkID = linkid.ToString();
                QueueItem item = new QueueItem(submit.OrdinalNumber);
                item.Sequence = submit.OrdinalNumber;
                item.msgObj = submit;
                item.msgType = 3;
                item.SqlID = (int) id;
                this.m_WaitSeqQueue.Add(item.Sequence, item);
            }
            catch (Exception ex /*FormatException */)
            {
                this.AddTextEvent( "出错了:" + ex.Message);
            }
        }

        private bool IsIncludeGB(string cont)
        {
            return (Encoding.Default.GetBytes(cont).Length > cont.Length);
        }

        private object LendData(IDataReader dr)
        {
            while ((dr != null) && dr.Read())
            {
                this.InQueueEvent(dr["id"], dr["SPNumber"], dr["ChargeNumber"], dr["UserCount"], dr["UserNumber"], dr["CorpId"], dr["ServiceType"], dr["FeeType"], dr["FeeValue"], dr["GivenValue"], dr["AgentFlag"], dr["MorelatetoMTFlag"], dr["Priority"], dr["ExpireTime"], dr["ScheduleTime"], dr["ReportFlag"], dr["TP_pid"], dr["TP_udhi"], dr["MessageCoding"], dr["MessageType"], dr["MessageContent"], dr["linkid"]);
            }
            return null;
        }

        private void MonitoringClientEvent()
        {
            if (this.m_TcpSocketClient.IsSend)
            {
                this.CheckReSend();
                TimeSpan span = (TimeSpan) (DateTime.Now - this.m_TcpSocketClient.LastTime);
                if (span.TotalSeconds >= 60.0)
                {
                    this.m_TcpSocketClient.CloseSocket();
                }
                this.m_MonitoringTime++;
            }
        }

        private void MonitoringServerEvent()
        {
            SortedList<string, ClientQueue> list = this.m_TcpSocketServer.ClientInfo.List;
            for (int i = 0; i < list.Count; i++)
            {
                ClientQueue queue = list.Values[i];
                TimeSpan span = (TimeSpan) (queue.Time - DateTime.Now);
                if (span.Seconds > 60L)
                {
                    this.m_TcpSocketServer.CloseSocket(queue.Soc);
                }
            }
        }

        private void ScanDataEvent()
        {
            if (this.m_OutSeqQueue.Count < 0x20L)
            {
                if (SmsFlagSolution == -1)
                {
                    this.m_DB.LendReader("exec sp_getdata", new BorrowReader(this.LendData));
                }
                else
                {
                    this.m_DB.LendReader("exec sp_getdata_safe  " + SmsFlagSolution.ToString(), new BorrowReader(this.LendData));
                }                
            }
        }

        private void SendSMSEvent()
        {
            try
            {
                QueueItem item = null;
                int num = 0;
                while (((item = this.m_WaitSeqQueue.GetTopOutQueue()) != null) && (num++ < 0x10))
                {
                    if (item.msgType == 3)
                    {
                        Submit msgObj = (Submit)item.msgObj;
                        this.m_TcpSocketClient.SendData(msgObj.toBytes());
                        this.m_WaitSeqQueue.Remove(item.Sequence);
                        this.m_OutSeqQueue.Add(item.Sequence, item);
                        SendMTCount++;
                        this.AddTextEvent("发送SGIP_SUBMIT    手机号：" + msgObj.UserNumber + ";内容:" + msgObj.MessageContent);

                        if (SmsFlagSolution != -1)
                        {
                            this.m_DB.ExeSQL(string.Format("update sgip_submit set smsflag=6 where id ={0}", item.SqlID));
                        }
                    }
                    else
                    {
                        this.m_WaitSeqQueue.Remove(item.Sequence);
                    }
                }
            }
            catch (SocketException)
            {
                this.AddTextEvent("请检查SMG设置是否正确，程序停止运行");                
                this.Stop();
            }            
        }

        private void ServerRecvEvent(byte[] msg, int recvCount, Socket client)
        {
            string sql = "";
            switch (BitConvert.bytes2Uint(msg, 4))
            {
                case 1:
                {
                    Bind bind = new Bind(msg, recvCount);
                    this.m_TcpSocketServer.SendMSG(new BindResp(bind.SequenceNumber).toBytes(), client);
                    return;
                }
                case 2:
                {
                    Unbind unbind = new Unbind(msg, recvCount);
                    this.m_TcpSocketServer.SendMSG(new UnbindResp(unbind.SequenceNumber).toBytes(), client);
                    this.m_TcpSocketServer.CloseSocket(client);
                    return;
                }
                case 4:
                {
                    Deliver deliver = new Deliver(msg, recvCount);
                    sql = string.Format("exec [sp_sms_Recv_Deliver] '{0}','{1}',{2},{3},'{4}','{5}'", new object[] { deliver.UserNumber, deliver.SPNumber, deliver.MessageCoding, deliver.MessageLength, deliver.MessageContent, deliver.LinkID });
                    this.m_DB.ExeSQL(sql);
                    this.AddTextEvent("接收到SGIP_DELIVER    手机号" + deliver.UserNumber + ";内容:" + deliver.MessageContent);
                    this.SmsResearchEvent(deliver.UserNumber, deliver.MessageContent);
                    this.m_TcpSocketServer.SendMSG(new DeliverResp(deliver.SequenceNumber).toBytes(), client);
                    RecvMOCount++;
                    return;
                }
                case 5:
                {
                    Report report = new Report(msg, recvCount);
                    sql = string.Format("exec sp_sms_Recv_Report {0},{1},{2},{3},'{4}',{5},{6},{7}", new object[] { report.NodeNumber, report.OldDateTimeNumber, report.OrdinalNumber, report.ReportType, report.UserNumber, report.State, report.ErrorCode, report.Reserve });
                    this.m_DB.ExeSQL(sql);
                    this.AddTextEvent(string.Concat(new object[] { "接收到SGIP_REPORT    手机号", report.UserNumber, ";状态:", report.ErrorCode }));
                    this.m_TcpSocketServer.SendMSG(new ReportResp(report.SequenceNumber).toBytes(), client);
                    return;
                }
                case 0x80000002:
                    this.m_TcpSocketServer.CloseSocket(client);
                    this.AddTextEvent("已经和SMG断开连接");
                    return;
            }
            this.AddTextEvent("未知消息");
        }

        public void Start()
        {
            try
            {
                if (!this.m_DB.CheckSQLConnect())
                {
                    this.AddTextEvent("请检查数据库设置");
                }
                else
                {
                    this.m_DB.ExeSQL("exec sp_sms_init");
                    if (this.StartEvent != null)
                    {
                        this.StartEvent();
                    }
                    this.m_SendSMSThread.StartThread();
                    this.m_MonitoringClientThread.StartThread();
                    this.m_ScanDataThread.StartThread();
                    this.m_MonitoringClientThread.StartThread();
                    this.m_TcpSocketServer.Start();
                }
            }
            catch (SocketException exception)
            {
                this.AddTextEvent(exception.Message);
            }
            catch (ObjectDisposedException exception2)
            {
                this.AddTextEvent(exception2.Message);
            }
        }

        public void Stop()
        {
            if (this.StopEvent != null)
            {
                this.StopEvent();
            }
            this.m_SendSMSThread.StopThread();
            this.m_MonitoringClientThread.StopThread();
            this.m_ScanDataThread.StopThread();
            this.m_MonitoringClientThread.StopThread();
            this.m_TcpSocketServer.Stop();
        }
    }
}

