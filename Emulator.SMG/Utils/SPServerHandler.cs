using SMG.SGIP.Base;
using SMG.SGIP.Command;
using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Emulator.SMG.Utils
{
    public class SPServerHandler
    {
        #region 变量

        TcpSocketServer tcpServer;
        List<SPClient> spClientPool;
        static SPServerHandler handler;

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

        public static void Register(VirtualGetway form, TcpSocketServer tcpServer)
        {
            handler = new SPServerHandler(form, tcpServer);
        }

        public static void Deliver(string spNumber, string userNumber, string content)
        {
            if (handler != null)
            {
                var client = handler.spClientPool.FirstOrDefault(i => i.SPNumber == spNumber);
                if (client != null)
                {
                    client.Socket.Send(new Deliver
                    {
                        SequenceNumber = Sequence.Next(),
                        SPNumber = spNumber,
                        UserNumber = userNumber,
                        MessageCoding = MessageCodes.GBK,
                        MessageContent = content,
                        TP_pid = 0,
                        TP_udhi = 0
                    }.GetBytes());
                }
            }
        }

        protected SPServerHandler(VirtualGetway form, TcpSocketServer tcpServer)
        {
            this.form = form;
            this.tcpServer = tcpServer;
            this.spClientPool = new List<SPClient>();
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
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    PrintLog("发送消息给 " + client.LocalIPAddress + " 出现错误：" + e.Message);
                }
            });
        }

        void OnException(Exception e)
        {
            ThreadCalls(() =>
            {
                MessageBox.Show(form, e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }

        #region resps

        void BindResp(TcpSocketClient client, Bind bind)
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

        void UnBindResp(TcpSocketClient client, UnBind ubind)
        {
            var resp = new UnBind_Resp()
            {
                SequenceNumber = ubind.SequenceNumber
            };

            client.Send(resp.GetBytes());
        }

        void SubmitResp(TcpSocketClient client, Submit submit)
        {
            var resp = new Submit_Resp
            {
                SequenceNumber = submit.SequenceNumber,
                Result = CommandError.Success
            };

            //todo message Verify
            client.Send(resp.GetBytes());
            //转发给手机客户端
            SMSCServerHandler.Deliver(submit.UserNumber, submit.SPNumber, submit.MessageContent);
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
                            this.BindResp(client, bind);
                            break;
                        case Commands.UnBind:
                            var unbind = new UnBind(buffers);
                            this.UnBindResp(client, unbind);
                            break;
                        case Commands.Submit:
                            var submit = new Submit(buffers);
                            this.SubmitResp(client, submit);
                            PrintLog("收到 " + submit.SPNumber + " 发送送给 " + submit.UserNumber + " 的消息： " + submit.MessageContent);
                            break;
                        default:
                            PrintLog("读取 " + client.LocalIPAddress + " 发送的命令：" + cmd.Command);
                            break;
                    }
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
                        PrintLog("读取 " + client.LocalIPAddress + " 发送的消息出现错误：" + e.Message);
                    }

                    //断开连接
                    client.Disconnect();
                }
            });
        }
    }
}
