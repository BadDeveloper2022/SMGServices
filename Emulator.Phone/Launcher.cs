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
    public partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        void ThreadCalls(Action action)
        {
            this.Invoke(action);
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
                btnOpen.Enabled = true;

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
                btnOpen.Enabled = false;

                lbStatus.Text = "未连接";
                lbAddress.Text = "0.0.0.0";
            });
        }

        void OnSend(TcpSocketClient client, byte[] buffers)
        {

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
                            SMSHistory.Add(new SMS
                            {
                                SPNumber = deliver.SPNumber,
                                UserNumber = deliver.UserNumber,
                                Content = deliver.MessageContent,
                                Time = DateTime.Now,
                                Type = SMSTypes.RECEIVE
                            });
                            //新消息提醒
                            MessageBox.Show(this, deliver.MessageContent, "新消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnOpen_Click(object sender, EventArgs e)
        {
            SMSSession sms = new SMSSession();
            sms.Bind();
            sms.ShowDialog(this);
        }
    }
}
