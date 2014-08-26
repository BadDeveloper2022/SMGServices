using SMG.SGIP.Base;
using SMG.SGIP.Command;
using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Emulator.SP
{
    public partial class VirtualSP : Form
    {
        public VirtualSP()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        void ThreadCalls(Action action)
        {
            this.Invoke(action);
        }

        void PrintLog(string log)
        {
            rtbLog.AppendText(DateTime.Now.ToString("HH:mm:ss") + " > " + log + "\n");
        }

        void BindSession(bool isSend, string userNumber, string content)
        {
            string number = userNumber;
            if (!isSend)
            {
                rtbSession.SelectionAlignment = HorizontalAlignment.Left;
            }
            else
            {
                number = cbbSPNumber.Text;
                rtbSession.SelectionAlignment = HorizontalAlignment.Right;
            }

            rtbSession.AppendText("(" + number + ")" + DateTime.Now.ToString("MM-dd HH:mm:ss") + "\n");
            rtbSession.SelectionColor = Color.Blue;
            rtbSession.SelectionFont = new Font(new FontFamily("宋体"), 9.5f, FontStyle.Bold);
            rtbSession.AppendText(content + "\n");
        }

        #region 变量

        TcpSocketClient client;

        #endregion

        #region TcpSocketClient事件处理

        void OnConnected(TcpSocketClient client)
        {
            client.Start();

            ThreadCalls(() =>
            {
                client.Send(new Bind()
                {
                    SequenceNumber = Sequence.Next(),
                    LoginType = LoginTypes.SPToSMG,
                    LoginName = cbbSPNumber.Text,
                    LoginPassword = cbbSPNumber.Text
                }.GetBytes());

                cbbSPNumber.Enabled = false;
                cbbSMGIP.Enabled = false;
                nudSMGPort.Enabled = false;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                btnSend.Enabled = true;

                lbStatus.Text = "已连接";
                lbAddress.Text = client.LocalIPAddress;
                this.Text = cbbSPNumber.Text;
            });
        }

        void OnDisconnected(TcpSocketClient client)
        {
            ThreadCalls(() =>
            {
                cbbSPNumber.Enabled = true;
                cbbSMGIP.Enabled = true;
                nudSMGPort.Enabled = true;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnSend.Enabled = false;

                lbStatus.Text = "未连接";
                lbAddress.Text = "0.0.0.0";

                rtbContent.Text = "";
                rtbSession.Text = "";

                PrintLog("已从服务器断开连接！");
            });
        }

        void OnSend(TcpSocketClient client, byte[] buffers)
        {
            ThreadCalls(() =>
            {
                try
                {
                    var cmd = new BaseCommand(buffers);
                    if (cmd.Command == Commands.Submit)
                    {
                        var submit = new Submit(buffers);
                        //绑定消息会话
                        BindSession(true, submit.UserNumber, submit.MessageContent);
                        rtbContent.Text = "";
                        cbbNumber.Text = "";
                        PrintLog("发送一条新消息给 " + submit.UserNumber);
                        MessageBox.Show(this, "发送成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    PrintLog("发送一条命令：" + Commands.GetString(cmd.Command));
                }
                catch (Exception e)
                {
                    PrintLog("发送消息出现错误： " + e.Message);
                }
            });
        }

        void OnRead(TcpSocketClient client, byte[] buffers)
        {
            ThreadCalls(() =>
            {
                try
                {
                    var cmd = new BaseCommand(buffers);

                    switch (cmd.Command)
                    {
                        case Commands.Bind_Resp:
                            var bind_resp = new Bind_Resp(buffers);
                            if (bind_resp.Result != CommandError.Success)
                            {
                                PrintLog("绑定验证失败：" + CommandError.GetMessage(bind_resp.Result));
                                Thread.Sleep(1000);
                                client.Disconnect();
                            }
                            break;
                        case Commands.Deliver:
                            var deliver = new Deliver(buffers);
                            //新消息提醒
                            PrintLog("收到一条 " + deliver.UserNumber + " 发来的新消息！");
                            //绑定消息会话
                            BindSession(false, deliver.UserNumber, deliver.MessageContent);
                            //发送响应
                            client.Send(new Deliver_Resp
                            {
                                SequenceNumber = deliver.SequenceNumber,
                                Result = CommandError.Success
                            }.GetBytes());
                            break;
                        case Commands.Deliver_Resp:
                            var deliver_resp = new Deliver_Resp(buffers);
                            if (deliver_resp.Result != CommandError.Success)
                            {
                                PrintLog("传送消息失败：" + CommandError.GetMessage(deliver_resp.Result));
                            }
                            break;
                        case Commands.Report:
                            client.Send(new Report_Resp
                            {
                                SequenceNumber = cmd.SequenceNumber,
                                Result = CommandError.Success
                            }.GetBytes());
                            break;
                        default:
                            break;
                    }

                    PrintLog("接收一条命令：" + Commands.GetString(cmd.Command));
                }
                catch (Exception e)
                {
                    PrintLog("读取消息出现错误： " + e.Message);
                }
            });
        }

        void OnException(Exception e)
        {
            ThreadCalls(() =>
            {
                MessageBox.Show(this, e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            });
        }

        #endregion


        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cbbSPNumber.Text == "" || cbbSMGIP.Text == "")
            {
                return;
            }

            if (client == null || !client.Connected)
            {
                client = new TcpSocketClient(cbbSMGIP.Text, (int)nudSMGPort.Value);
                client.OnConnected += OnConnected;
                client.OnDisconnected += OnDisconnected;
                client.OnRead += OnRead;
                client.OnSend += OnSend;
                client.OnException += OnException;
                client.Connect();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                client.Disconnect();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (cbbNumber.Text == "" || rtbContent.Text == "")
            {
                return;
            }

            if (client != null)
            {
                var submit = new Submit()
                {
                    SPNumber = cbbSPNumber.Text,
                    UserCount = 1,
                    UserNumber = cbbNumber.Text,
                    CorpId = "0591",
                    ExpireTime = "",
                    ScheduleTime = "",
                    ServiceType = "test",
                    FeeType = FeeTypes.SendFree,
                    FeeValue = "000000",
                    GivenValue = "000000",
                    MessageCoding = MessageCodes.GBK,
                    MessageType = 0,
                    MorelatetoMTFlag = 0,
                    TP_pid = 0,
                    TP_udhi = 0,
                    Priority = 1,
                    ReportFlag = 0,
                    MessageContent = rtbContent.Text,
                    ChargeNumber = cbbSPNumber.Text,
                    AgentFlag = 1,
                };

                client.Send(submit.GetBytes());
            }
        }
    }
}
