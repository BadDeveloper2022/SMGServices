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
    public class SMSCServerHandler
    {
        #region 变量

        TcpSocketServer tcpServer;
        List<MTClient> mtClientPool;
        static SMSCServerHandler handler;

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

        public static void Register(VirtualGetway form, TcpSocketServer tcpServer)
        {
            handler = new SMSCServerHandler(form, tcpServer);
        }

        public static void Deliver(string userNumber, string spNumber, string content)
        {
            if (handler != null)
            {
                var client = handler.mtClientPool.FirstOrDefault(i => i.UserNumber == userNumber);
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

        protected SMSCServerHandler(VirtualGetway form, TcpSocketServer tcpServer)
        {
            this.form = form;
            this.tcpServer = tcpServer;
            this.mtClientPool = new List<MTClient>();
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

        void UnBindResp(TcpSocketClient client, UnBind ubind)
        {
            var resp = new UnBind_Resp()
            {
                SequenceNumber = ubind.SequenceNumber
            };

            client.Send(resp.GetBytes());
        }

        void DeliverResp(TcpSocketClient client, Deliver deliver)
        {
            var resp = new Deliver_Resp
            {
                SequenceNumber = deliver.SequenceNumber,
                Result = CommandError.Success
            };

            client.Send(resp.GetBytes());
            //转发给SP客户端
            SPServerHandler.Deliver(deliver.SPNumber, deliver.UserNumber, deliver.MessageContent);
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
                        case Commands.Deliver:
                            var deliver = new Deliver(buffers);
                            this.DeliverResp(client, deliver);
                            PrintLog("收到 " + deliver.UserNumber + " 发送送给 " + deliver.SPNumber + " 的消息： " + deliver.MessageContent);
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
