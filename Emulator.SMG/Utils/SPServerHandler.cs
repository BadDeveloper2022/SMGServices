using SMG.SGIP.Base;
using SMG.SGIP.Command;
using SMG.Sqlite;
using SMG.Sqlite.Models;
using SMG.Sqlite.Storage;
using SMG.TcpSocket;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace Emulator.SMG.Utils
{
    public class SPServerHandler
    {
        #region 变量

        TcpSocketServer tcpServer;
        List<SPClient> spClientPool;
        Dictionary<string, string> sequeueDict;

        #endregion

        #region 控件

        VirtualGetway form;
        Button btnSPStop;
        Button btnSPStart;
        ComboBox cbbSPIP;
        ListBox lbSPList;
        NumericUpDown nudSPCount;
        NumericUpDown nudSPPort;
        RichTextBox rtbSPLog;

        #endregion

        public SPClient GetSPClient(string spNumber)
        {
            return spClientPool.FirstOrDefault(i => i.SPNumber == spNumber);
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

        public SPServerHandler(VirtualGetway form, TcpSocketServer tcpServer)
        {
            this.form = form;
            this.tcpServer = tcpServer;
            this.spClientPool = new List<SPClient>();
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
            btnSPStop = (Button)form.Controls.Find("btnSPStop", true)[0];
            btnSPStart = (Button)form.Controls.Find("btnSPStart", true)[0];
            cbbSPIP = (ComboBox)form.Controls.Find("cbbSPIP", true)[0];
            lbSPList = (ListBox)form.Controls.Find("lbSPList", true)[0];
            nudSPCount = (NumericUpDown)form.Controls.Find("nudSPCount", true)[0];
            nudSPPort = (NumericUpDown)form.Controls.Find("nudSPPort", true)[0];
            rtbSPLog = (RichTextBox)form.Controls.Find("rtbSPLog", true)[0];
            ToolTip tt = new ToolTip();

            lbSPList.SelectedIndexChanged += (sender, e) =>
            {
                if (lbSPList.SelectedItem != null)
                {
                    var client = spClientPool.FirstOrDefault(i => i.Socket.LocalIPAddress == lbSPList.SelectedItem.ToString());
                    if (client != null)
                    {
                        tt.Show(client.SPNumber, lbSPList);
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
            rtbSPLog.AppendText(DateTime.Now.ToString("HH:mm:ss") + " > " + log + "\n");
        }

        void OnStart(TcpSocketServer server)
        {
            ThreadCalls(() =>
            {
                cbbSPIP.Enabled = false;
                nudSPPort.Enabled = false;
                nudSPCount.Enabled = false;
                btnSPStart.Enabled = false;
                btnSPStop.Enabled = true;
                PrintLog("启动SP网关服务成功！");
            });
        }

        void OnStop(TcpSocketServer server)
        {
            for (int i = 0; i < spClientPool.Count; i++)
            {
                var socket = spClientPool[i].Socket;
                socket.Send(new UnBind
                {
                    SequenceNumber = Sequence.Next()
                }.GetBytes());
            }

            MessageCenter.GetInstance().UnRegisterSP();

            ThreadCalls(() =>
            {
                cbbSPIP.Enabled = true;
                nudSPPort.Enabled = true;
                nudSPCount.Enabled = true;
                btnSPStart.Enabled = true;
                btnSPStop.Enabled = false;
                spClientPool.Clear();

                lbSPList.Items.Clear();
                PrintLog("停止SP网关服务成功！");
            });
        }

        void OnConnected(TcpSocketClient client)
        {
            ThreadCalls(() =>
            {
                PrintLog(client.LocalIPAddress + " 已连接！");

                lbSPList.Items.Add(client.LocalIPAddress);
            });
        }

        void OnDisconnected(TcpSocketClient client)
        {
            ThreadCalls(() =>
            {
                lbSPList.Items.Remove(client.LocalIPAddress);
                spClientPool.RemoveAll(i => i.Socket == client);
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
                var spNumber = spClientPool.First(i => i.Socket == client).SPNumber;
                //读取为发送的消息
                var deliverDao = StorageProvider<DeliverStorage>.GetStorage();
                var deliverList = deliverDao.GetList(spNumber);

                if (deliverList.Count() > 0)
                {
                    foreach (var deliver in deliverList)
                    {
                        var d = new Deliver
                        {
                            SPNumber = deliver.SPNumber,
                            UserNumber = deliver.UserNumber,
                            TP_pid = 0,
                            TP_udhi = 0,
                            MessageCoding = MessageCodes.GBK,
                            MessageContent = deliver.Content
                        };

                        client.Send(d.GetBytes());
                        //映射序列号
                        MapSequeue(d.SequenceNumberString, deliver.SequenceNumber);
                    }
                }

                //读取上次未发送的报告
                var reportDao = StorageProvider<ReportStorage>.GetStorage();
                var reportList = reportDao.GetList(spNumber);

                if (reportList.Count() > 0)
                {
                    foreach (var report in reportList)
                    {
                        var r = new Report
                        {
                            SubmitSequenceNumber = report.TargetSubmitSequenceNumber,
                            ReportType = (uint)report.ReportType,
                            State = (uint)report.State,
                            ErrorCode = (uint)report.ErrorCode,
                            UserNumber = report.UserNumber
                        };

                        client.Send(r.GetBytes());
                        //映射序列号
                        MapSequeue(r.SequenceNumberString, report.SubmitSequenceNumber);
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

            if (bind.LoginType != LoginTypes.SPToSMG)
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
                else if (spClientPool.Count(i => i.SPNumber.Equals(bind.LoginName)) > 0)
                {
                    resp.Result = CommandError.RepeatLogin;
                }
                else
                {
                    //添加到连接池
                    spClientPool.Add(new SPClient
                    {
                        SPNumber = bind.LoginName,
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

        void SubmitHandler(TcpSocketClient client, Submit submit)
        {
            var resp = new Submit_Resp
            {
                SequenceNumber = submit.SequenceNumber,
                Result = CommandError.Success
            };

            client.Send(resp.GetBytes());

            //添加SP发送的Submit消息到数据库
            var mSubmit = new MSubmit
            {
                TargetSequenceNumber = submit.SequenceNumber,
                SequenceNumber = submit.SequenceNumberString,
                SPNumber = submit.SPNumber,
                UserNumber = submit.UserNumber,
                ReportFlag = (int)submit.ReportFlag,
                Content = submit.MessageContent,
                Created = DateTime.Now,
                Status = 0
            };
            StorageProvider<SubmitStorage>.GetStorage().Insert(mSubmit);

            //转发给消息中心处理
            MessageCenter.GetInstance().Commit(submit);
        }

        void DeliverRespHandler(TcpSocketClient client, Deliver_Resp resp)
        {
            var seq = GetSequeue(resp.SequenceNumberString);
            if (seq != null)
            {
                StorageProvider<DeliverStorage>.GetStorage().Update(seq, 1);
            }
        }

        void ReportRespHandler(TcpSocketClient client, Report_Resp resp)
        {
            var seq = GetSequeue(resp.SequenceNumberString);
            if (seq != null)
            {
                var dao = StorageProvider<ReportStorage>.GetStorage();
                var mReport = dao.Get(seq);
                if (mReport != null)
                {
                    mReport.Status = 1;
                    dao.Update(mReport);
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
                        case Commands.Submit:
                            var submit = new Submit(buffers);
                            this.SubmitHandler(client, submit);
                            PrintLog("接收到 \"" + submit.SPNumber + "\" 发送送给 \"" + submit.UserNumber + "\" 的消息： " + submit.MessageContent);
                            break;
                        case Commands.Deliver_Resp:
                            this.DeliverRespHandler(client, new Deliver_Resp(buffers));
                            break;
                        case Commands.Report_Resp:
                            this.ReportRespHandler(client, new Report_Resp(buffers));
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
