using SMG.SGIP.Base;
using SMG.SGIP.Command;
using SMG.Sqlite;
using SMG.Sqlite.Models;
using SMG.Sqlite.Storage;
using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace Emulator.SMG.Utils
{
    public class SMSCServerHandler
    {
        #region 变量

        TcpSocketServer tcpServer;
        List<MTClient> mtClientPool;
        Dictionary<string, string> sequeueDict;

        #endregion

        #region 控件

        VirtualGetway form;
        Button btnSMSCStop;
        Button btnSMSCStart;
        ComboBox cbbSMSCIP;
        ListBox lbPhoneList;
        NumericUpDown nudSMSCCount;
        NumericUpDown nudSMSCPort;
        RichTextBox rtbSMSCLog;

        #endregion

        public MTClient GetMTClient(string userNumber)
        {
            return mtClientPool.FirstOrDefault(i => i.UserNumber == userNumber);
        }

        public void MapSequeue(string seq, string mapSeq)
        {
            if (!sequeueDict.ContainsKey(seq))
            {
                sequeueDict.Add(seq, mapSeq);
            }
        }

        string GetSequeue(string seq)
        {
            if (sequeueDict.ContainsKey(seq))
            {
                return sequeueDict[seq];
            }

            return null;
        }

        public SMSCServerHandler(VirtualGetway form, TcpSocketServer tcpServer)
        {
            this.form = form;
            this.tcpServer = tcpServer;
            this.mtClientPool = new List<MTClient>();
            sequeueDict = new Dictionary<string, string>();
            this.OnCreate();

            tcpServer.OnConnected += this.OnConnected;
            tcpServer.OnDisconnected += this.OnDisconnected;
            tcpServer.OnStart += this.OnStart;
            tcpServer.OnStop += this.OnStop;
            tcpServer.OnRead += this.OnRead;
            tcpServer.OnSend += this.OnSend;
            tcpServer.OnException += this.OnException;
        }

        void OnCreate()
        {
            btnSMSCStop = (Button)form.Controls.Find("btnSMSCStop", true)[0];
            btnSMSCStart = (Button)form.Controls.Find("btnSMSCStart", true)[0];
            cbbSMSCIP = (ComboBox)form.Controls.Find("cbbSMSCIP", true)[0];
            lbPhoneList = (ListBox)form.Controls.Find("lbPhoneList", true)[0];
            nudSMSCCount = (NumericUpDown)form.Controls.Find("nudSMSCCount", true)[0];
            nudSMSCPort = (NumericUpDown)form.Controls.Find("nudSMSCPort", true)[0];
            rtbSMSCLog = (RichTextBox)form.Controls.Find("rtbSMSCLog", true)[0];
            ToolTip tt = new ToolTip();

            lbPhoneList.SelectedIndexChanged += (sender, e) =>
            {
                if (lbPhoneList.SelectedItem != null)
                {
                    var client = mtClientPool.FirstOrDefault(i => i.Socket.LocalIPAddress == lbPhoneList.SelectedItem.ToString());
                    if (client != null)
                    {
                        tt.Show(client.UserNumber, lbPhoneList);
                    }
                }
            };
        }

        void ThreadCalls(Action action)
        {
            form.Invoke(action);
        }

        void PrintLog(string log)
        {
            rtbSMSCLog.AppendText(DateTime.Now.ToString("HH:mm:ss") + " > " + log + "\n");
        }

        void OnStart(TcpSocketServer server)
        {
            ThreadCalls(() =>
            {
                cbbSMSCIP.Enabled = false;
                nudSMSCCount.Enabled = false;
                nudSMSCPort.Enabled = false;
                btnSMSCStart.Enabled = false;
                btnSMSCStop.Enabled = true;
                PrintLog("启动SMSC网关服务成功！");
            });
        }

        void OnStop(TcpSocketServer server)
        {
            for (int i = 0; i < mtClientPool.Count; i++)
            {
                var socket = mtClientPool[i].Socket;
                socket.Send(new UnBind
                {
                    SequenceNumber = Sequence.Next()
                }.GetBytes());
            }

            MessageCenter.GetInstance().UnRegisterSMSC();

            ThreadCalls(() =>
            {
                cbbSMSCIP.Enabled = true;
                nudSMSCCount.Enabled = true;
                nudSMSCPort.Enabled = true;
                btnSMSCStart.Enabled = true;
                btnSMSCStop.Enabled = false;

                lbPhoneList.Items.Clear();
                PrintLog("停止SMSC网关服务成功！");
            });
        }

        void OnConnected(TcpSocketClient client)
        {
            ThreadCalls(() =>
            {
                PrintLog(client.LocalIPAddress + " 已连接！");
                lbPhoneList.Items.Add(client.LocalIPAddress);
            });
        }

        void OnDisconnected(TcpSocketClient client)
        {
            ThreadCalls(() =>
            {
                lbPhoneList.Items.Remove(client.LocalIPAddress);
                mtClientPool.RemoveAll(i => i.Socket == client);
                PrintLog(client.LocalIPAddress + " 已断开连接！");
            });
        }

        void OnException(Exception e)
        {
            ThreadCalls(() =>
            {
                MessageBox.Show(form, e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }

        void StartWaitSendThread(TcpSocketClient client)
        {
            //开启线程读取上次未发送的消息
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                var userNumber = mtClientPool.First(i => i.Socket == client).UserNumber;
                var dao = StorageProvider<SubmitStorage>.GetStorage();
                var list = dao.GetList(userNumber);

                if (list.Count() > 0)
                {

                    foreach (var submit in list)
                    {
                        var d = new Deliver
                        {
                            SPNumber = submit.SPNumber,
                            UserNumber = submit.UserNumber,
                            TP_pid = 0,
                            TP_udhi = 0,
                            MessageCoding = MessageCodes.GBK,
                            MessageContent = submit.Content
                        };

                        client.Send(d.GetBytes());
                        //映射序列号
                        MapSequeue(d.SequenceNumberString, submit.SequenceNumber);
                    }
                }
            });
        }

        void OnSend(TcpSocketClient client, byte[] buffers)
        {
            ThreadCalls(() =>
            {
                try
                {
                    BaseCommand cmd = new BaseCommand(buffers);
                    switch (cmd.Command)
                    {
                        case Commands.UnBind:
                        case Commands.UnBind_Resp:
                            client.Disconnect();
                            break;
                        case Commands.Bind_Resp:
                            //绑定验证出错则断开连接
                            var bindresp = new Bind_Resp(buffers);
                            if (bindresp.Result != CommandError.Success)
                            {
                                client.Disconnect();
                            }
                            else
                            {
                                StartWaitSendThread(client);
                            }
                            break;
                        default:
                            break;
                    }

                    PrintLog("发送一条命令 \"" + Commands.GetString(cmd.Command) + "\" 给 \"" + client.LocalIPAddress + "\"");
                }
                catch (Exception e)
                {
                    PrintLog("发送一条命令给 \"" + client.LocalIPAddress + "\" 出现错误：" + e.Message);
                }
            });
        }

        #region command handler

        void BindHandler(TcpSocketClient client, Bind bind)
        {
            var resp = new Bind_Resp
            {
                SequenceNumber = bind.SequenceNumber,
                Result = CommandError.Success
            };

            if (bind.LoginType != LoginTypes.Test)
            {
                resp.Result = CommandError.InvalidLoginType;
            }
            else
            {
                //todo auth
                if (bind.LoginName == "" || bind.LoginPassword == "")
                {
                    resp.Result = CommandError.InvalidLogin;
                }
                else if (mtClientPool.Count(i => i.UserNumber.Equals(bind.LoginName)) > 0)
                {
                    resp.Result = CommandError.RepeatLogin;
                }
                else
                {
                    //添加到连接池
                    mtClientPool.Add(new MTClient
                    {
                        UserNumber = bind.LoginName,
                        Socket = client
                    });
                }
            }

            client.Send(resp.GetBytes());
        }

        void UnBindHandler(TcpSocketClient client, UnBind ubind)
        {
            var resp = new UnBind_Resp()
            {
                SequenceNumber = ubind.SequenceNumber
            };

            client.Send(resp.GetBytes());
        }

        void DeliverHandler(TcpSocketClient client, Deliver deliver)
        {
            var resp = new Deliver_Resp
            {
                SequenceNumber = deliver.SequenceNumber,
                Result = CommandError.Success
            };

            client.Send(resp.GetBytes());

            //将seq设置为SMG的seq
            deliver.SequenceNumber = Sequence.Next();
            //插入手机发送的Deliver消息到数据库
            var mDeliver = new MDeliver
            {
                SequenceNumber = deliver.SequenceNumberString,
                TargetSequenceNumber = deliver.SequenceNumber,
                SPNumber = deliver.SPNumber,
                UserNumber = deliver.UserNumber,
                Content = deliver.MessageContent,
                Created = DateTime.Now,
                Status = 0
            };
            StorageProvider<DeliverStorage>.GetStorage().Insert(mDeliver);
            
            //转发给消息中心处理
            MessageCenter.GetInstance().Commit(deliver);
        }

        void DeliverRespHandler(TcpSocketClient client, Deliver_Resp resp)
        {
            var seq = GetSequeue(resp.SequenceNumberString);
            if (seq != null)
            {
                StorageProvider<SubmitStorage>.GetStorage().Update(seq, 1);
                var mReport = StorageProvider<ReportStorage>.GetStorage().Get(seq);
                if (mReport != null)
                {
                    mReport.State = (int)ReportStatus.Success;
                    StorageProvider<ReportStorage>.GetStorage().Update(mReport);

                    //转发给消息中心处理
                    MessageCenter.GetInstance().Commit(mReport);
                }
            }
        }

        #endregion

        void OnRead(TcpSocketClient client, byte[] buffers)
        {
            ThreadCalls(() =>
            {
                try
                {
                    BaseCommand cmd = new BaseCommand(buffers);
                    switch (cmd.Command)
                    {
                        case Commands.Bind:
                            var bind = new Bind(buffers);
                            this.BindHandler(client, bind);
                            break;
                        case Commands.UnBind:
                            var unbind = new UnBind(buffers);
                            this.UnBindHandler(client, unbind);
                            break;
                        case Commands.Deliver:
                            var deliver = new Deliver(buffers);
                            this.DeliverHandler(client, deliver);
                            PrintLog("接收到一条 \"" + deliver.UserNumber + "\" 发送送给 \"" + deliver.SPNumber + "\" 的消息： " + deliver.MessageContent);
                            break;
                        case Commands.Deliver_Resp:
                            this.DeliverRespHandler(client, new Deliver_Resp(buffers));
                            break;
                        default:
                            break;
                    }

                    PrintLog("接收到一条 \"" + client.LocalIPAddress + "\" 发送的命令：" + Commands.GetString(cmd.Command));
                }
                catch (Exception e)
                {
                    if (e is BadCmdHeadException)
                    {
                        PrintLog(client.LocalIPAddress + " 发送错误的消息头格式！");
                    }
                    else if (e is BadCmdBodyException)
                    {
                        PrintLog(client.LocalIPAddress + " 发送错误的消息体格式！");
                    }
                    else
                    {
                        PrintLog("接受 \"" + client.LocalIPAddress + "\" 发送的消息出现错误：" + e.Message);
                    }

                    //断开连接
                    client.Disconnect();
                }
            });
        }

    }
}
