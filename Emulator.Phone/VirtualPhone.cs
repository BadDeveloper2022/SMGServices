using Emulator.Phone.Utils;
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
using System.Windows.Forms;

namespace Emulator.Phone
{
    public partial class VirtualPhone : Form
    {
        public VirtualPhone()
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
            rtbLog.AppendText(">" + log + "\n");
        }

        void BindGroup(string spNumber)
        {
            if (!lbGroup.Items.Contains(spNumber))
            {
                lbGroup.Items.Add(spNumber);
            }
        }

        void BindSession(SMS s)
        {
            if (s.Type == SMSTypes.RECEIVE)
            {
                rtbSession.SelectionAlignment = HorizontalAlignment.Left;
            }
            else
            {
                rtbSession.SelectionAlignment = HorizontalAlignment.Right;
            }

            rtbSession.AppendText(s.Time.ToString("MM-dd HH:mm:ss") + "\n");
            rtbSession.SelectionColor = Color.Blue;
            rtbSession.SelectionFont = new Font(new FontFamily("宋体"), 9.5f, FontStyle.Bold);
            rtbSession.AppendText(s.Content + "\n");
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
                    LoginType = LoginTypes.Test,
                    LoginName = cbbNumber.Text,
                    LoginPassword = cbbNumber.Text
                }.GetBytes());

                cbbNumber.Enabled = false;
                cbbSMGIP.Enabled = false;
                cbbSMGPort.Enabled = false;
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                btnSend.Enabled = true;

                lbStatus.Text = "已连接";
                lbAddress.Text = client.LocalIPAddress;
                this.Text = cbbNumber.Text;
            });
        }

        void OnDisconnected(TcpSocketClient client)
        {
            ThreadCalls(() =>
            {
                cbbNumber.Enabled = true;
                cbbSMGIP.Enabled = true;
                cbbSMGPort.Enabled = true;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnSend.Enabled = false;

                lbStatus.Text = "未连接";
                lbAddress.Text = "0.0.0.0";

                lbGroup.Items.Clear();
                rtbContent.Text = "";
            });
        }

        void OnSend(TcpSocketClient client, byte[] buffers)
        {
            ThreadCalls(() =>
            {
                try
                {
                    var cmd = new BaseCommand(buffers);
                    if (cmd.Command == Commands.Deliver)
                    {
                        var deliver = new Deliver(buffers);

                        var sms = new SMS
                        {
                            SPNumber = deliver.SPNumber,
                            UserNumber = deliver.UserNumber,
                            Content = deliver.MessageContent,
                            Time = DateTime.Now,
                            Type = SMSTypes.SEND
                        };
                        SMSHistory.Add(sms);

                        BindGroup(deliver.SPNumber);

                        if (lbGroup.SelectedItem != null && lbGroup.SelectedItem.ToString() == cbbSPNumber.Text)
                        {
                            BindSession(sms);
                        }

                        rtbContent.Text = "";
                        cbbSPNumber.Text = "";
                        PrintLog(DateTime.Now.ToString("HH:mm:ss") + " 发送一条新消息给 " + sms.SPNumber);
                        MessageBox.Show(this, "发送成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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
                            var bind_resp = new Deliver_Resp(buffers);
                            Console.WriteLine(bind_resp.ToString());
                            break;
                        case Commands.Deliver:
                            var deliver = new Deliver(buffers);
                            //添加接收记录
                            var sms = new SMS
                            {
                                SPNumber = deliver.SPNumber,
                                UserNumber = deliver.UserNumber,
                                Content = deliver.MessageContent,
                                Time = DateTime.Now,
                                Type = SMSTypes.RECEIVE
                            };
                            SMSHistory.Add(sms);
                            //绑定会话组
                            BindGroup(deliver.SPNumber);
                            if (lbGroup.SelectedItem != null && lbGroup.SelectedItem.ToString() == sms.SPNumber)
                            {
                                BindSession(sms);
                            }
                            //新消息提醒
                            PrintLog(DateTime.Now.ToString("HH:mm:ss") + " 收到一条 " + sms.SPNumber + " 发来的新消息！");
                            break;
                        case Commands.Deliver_Resp:
                            var deliver_resp = new Deliver_Resp(buffers);
                            Console.WriteLine(deliver_resp.ToString());
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
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
            if (cbbNumber.Text == "" || cbbSMGIP.Text == "" || cbbSMGPort.Text == "")
            {
                return;
            }

            if (client == null || !client.Connected)
            {
                client = new TcpSocketClient(cbbSMGIP.Text, int.Parse(cbbSMGPort.Text));
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
            if (cbbSPNumber.Text == "" || rtbContent.Text == "")
            {
                return;
            }

            if (client != null)
            {
                client.Send(new Deliver
                {
                    SequenceNumber = Sequence.Next(),
                    SPNumber = cbbSPNumber.Text,
                    MessageContent = rtbContent.Text,
                    MessageCoding = MessageCodes.GBK,
                    UserNumber = cbbNumber.Text,
                    TP_pid = 0,
                    TP_udhi = 0
                }.GetBytes());
            }
        }

        private void lbGroup_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lbGroup.SelectedIndex < 0) return;

            string spNumber = lbGroup.SelectedItem.ToString();
            var session = SMSHistory.GetSession(spNumber);
            if (session != null)
            {
                gpSession.Text = spNumber + " 会话";
                this.rtbSession.Text = "";

                foreach (var s in session)
                {
                    BindSession(s);
                }
            }
        }

    }
}
