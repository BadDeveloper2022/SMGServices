using SMG.SGIP.Base;
using SMG.SGIP.Command;
using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.CmdApp
{
    public class CmdRouter
    {
        public void Register(TcpSocketServer tcpServer)
        {
            tcpServer.OnRecv += Router;
            tcpServer.OnSend += OnSend;
        }

        #region routers

        private void BindRouter(TcpSocketClient client, Bind bind)
        {
            var resp = new Bind_Resp
            {
                SequenceNumber = bind.SequenceNumber,
                Result = CommandError.Success
            };

            switch (bind.LoginType)
            {
                case LoginTypes.SPToSMG:
                case LoginTypes.SMGToSP:
                case LoginTypes.SMGToSMG:
                case LoginTypes.SMGToGNS:
                case LoginTypes.GNSToSMG:
                case LoginTypes.GNSToGNS:
                case LoginTypes.Test:
                    break;
                default:
                    resp.Result = CommandError.InvalidLoginType;
                    break;
            }
            //todo auth
            if (bind.LoginName == "" || bind.LoginPassword == "")
            {
                resp.Result = CommandError.InvalidLogin;
            }

            client.Send(resp.GetBytes());

            //output
            Console.WriteLine(client.LocalIPAddress + " Send <Bind> ：\n" + bind.ToString());
            Console.WriteLine("Server Send <Bind_Resp> ：\n" + resp.ToString());
        }

        private void SubmitRouter(TcpSocketClient client, Submit submit)
        {
            var resp = new Submit_Resp
            {
                SequenceNumber = submit.SequenceNumber,
                Result = CommandError.Success
            };

            //todo message Verify

            client.Send(resp.GetBytes());

            //output
            Console.WriteLine(client.LocalIPAddress + " Send <Submit> ：\n" + submit.ToString());
            Console.WriteLine("Server Send <Submit_Resp> ：\n" + resp.ToString());
        }

        private void DeliverRouter(TcpSocketClient client, Deliver deliver)
        {
            var resp = new Deliver_Resp
            {
                SequenceNumber = deliver.SequenceNumber,
                Result = CommandError.Success
            };

            client.Send(resp.GetBytes());

            //output
            Console.WriteLine(client.LocalIPAddress + " Send <Deliver> ：\n" + deliver.ToString());
            Console.WriteLine("Server Send <Deliver_Resp> ：\n" + resp.ToString());
        }

        private void ReportRouter(TcpSocketClient client, Report report)
        {
            var resp = new Deliver_Resp
            {
                SequenceNumber = report.SequenceNumber,
                Result = CommandError.Success
            };

            client.Send(resp.GetBytes());

            //output
            Console.WriteLine(client.LocalIPAddress + " Send <Report> ：\n" + report.ToString());
            Console.WriteLine("Server Send <Report_Resp> ：\n" + resp.ToString());
        }

        private void UserRptRouter(TcpSocketClient client, UserRpt user)
        {
            var resp = new UserRpt_Resp
            {
                SequenceNumber = user.SequenceNumber,
                Result = CommandError.Success
            };

            client.Send(resp.GetBytes());

            //output
            Console.WriteLine(client.LocalIPAddress + " Send <UserRpt> ：\n" + user.ToString());
            Console.WriteLine("Server Send <UserRpt_Resp> ：\n" + resp.ToString());
        }

        private void CheckUserRouter(TcpSocketClient client, CheckUser check)
        {
            var resp = new Deliver_Resp
            {
                SequenceNumber = check.SequenceNumber,
                Result = CommandError.Success
            };

            client.Send(resp.GetBytes());

            //output
            Console.WriteLine(client.LocalIPAddress + " Send <CheckUser> ：\n" + check.ToString());
            Console.WriteLine("Server Send <CheckUser_Resp> ：\n" + resp.ToString());
        }

        private void TraceRouter(TcpSocketClient client, Trace trace)
        {
            var resp = new Deliver_Resp
            {
                SequenceNumber = trace.SequenceNumber,
                Result = CommandError.Success
            };

            client.Send(resp.GetBytes());

            //output
            Console.WriteLine(client.LocalIPAddress + " Send <Trace> ：\n" + trace.ToString());
            Console.WriteLine("Server Send <Trace_Resp> ：\n" + resp.ToString());
        }

        #endregion

        public void Router(TcpSocketClient client, byte[] buffers)
        {
            try
            {
                BaseCommand cmd = new BaseCommand(buffers);
                switch (cmd.Command)
                {
                    case Commands.Bind:
                        var bind = new Bind(buffers);
                        this.BindRouter(client, bind);
                        break;
                    case Commands.UnBind:
                        client.Send(new UnBind_Resp()
                        {
                            SequenceNumber = cmd.SequenceNumber
                        }.GetBytes());
                        break;
                    case Commands.Submit:
                        var submit = new Submit(buffers);
                        this.SubmitRouter(client, submit);
                        break;
                    case Commands.Deliver:
                        var deliver = new Deliver(buffers);
                        this.DeliverRouter(client, deliver);
                        break;
                    case Commands.Report:
                        var report = new Report(buffers);
                        this.ReportRouter(client, report);
                        break;
                    case Commands.UserRpt:
                        var user = new UserRpt(buffers);
                        this.UserRptRouter(client, user);
                        break;
                    case Commands.CheckUser:
                        var check = new CheckUser(buffers);
                        this.CheckUserRouter(client, check);
                        break;
                    case Commands.Trace:
                        var trace = new Trace(buffers);
                        this.TraceRouter(client, trace);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                if (e is BadCmdHeadException)
                {
                    Console.WriteLine(client.LocalIPAddress + " 发送错误的消息头格式！");
                }
                else if (e is BadCmdBodyException)
                {
                    Console.WriteLine(client.LocalIPAddress + " 发送错误的消息体格式！");
                }
                else
                {
                    Console.WriteLine(client.LocalIPAddress + " 发送消息错误 ： " + e.Message);
                }
            }
        }

        #region senders


        #endregion

        public void OnSend(TcpSocketClient client, byte[] buffers)
        {
            try
            {
                BaseCommand cmd = new BaseCommand(buffers);
                switch (cmd.Command)
                {
                    case Commands.UnBind:
                        client.Disconnect();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                if (e is BadCmdHeadException)
                {
                    Console.WriteLine("给 " + client.LocalIPAddress + " 发送的消息头格式错误！");
                }
                else if (e is BadCmdBodyException)
                {
                    Console.WriteLine("给 " + client.LocalIPAddress + " 发送的消息体格式错误！");
                }
                else
                {
                    Console.WriteLine("给 " + client.LocalIPAddress + " 发送的消息出错 ： " + e.Message);
                }
            }
        }
    }
}
