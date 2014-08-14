using SMG.SGIP.Base;
using SMG.SGIP.Command;
using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Cmd.SMG
{
    public class SMGToSPHandler
    {
        static Dictionary<string, string> SPDict = new Dictionary<string, string>();
        static SMGToSPHandler()
        {
            SPDict.Add("fssw2hy", "106559284130016");
            SPDict.Add("test", "10086");
        }

        private TcpSocketServer tcpServer;
        private string title = "";
        private List<SPClient> spPool;
        private List<MTClient> mtPool;

        public SMGToSPHandler(List<SPClient> spPool, List<MTClient> mtPool)
        {
            this.spPool = spPool;
            this.mtPool = mtPool;
        }

        public void Register(TcpSocketServer tcpServer)
        {
            this.tcpServer = tcpServer;
            this.title = tcpServer.BindIPAddress + "(SMGToSP)>";
            tcpServer.OnConnected += this.OnConnected;
            tcpServer.OnDisconnected += this.OnDisconnected;
            tcpServer.OnStop += this.OnStop;
            tcpServer.OnRead += this.OnRead;
            tcpServer.OnSend += this.OnSend;
        }

        public void OnStop()
        {
            for (int i = 0; i < spPool.Count; i++)
            {
                var socket = spPool[i].Socket;
                socket.Send(new UnBind
                {
                    SequenceNumber = Sequence.Next()
                }.GetBytes());
            }
            Console.WriteLine(title + "已停止服务！");
        }

        public void OnConnected(TcpSocketClient client)
        {
            Console.WriteLine(title + client.LocalIPAddress + " 已连接");
        }

        public void OnDisconnected(TcpSocketClient client)
        {
            spPool.RemoveAll(i => i.Socket == client);
            Console.WriteLine(title + client.LocalIPAddress + " 已断开连接");
        }

        public void OnSend(TcpSocketClient client, byte[] buffers)
        {
            try
            {
                BaseCommand cmd = new BaseCommand(buffers);
                switch (cmd.Command)
                {
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
                Console.WriteLine(title + e);
            }
        }

        #region resps

        private void BindResp(TcpSocketClient client, Bind bind)
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

                SPClient sp = new SPClient
                {
                    SPNumber = SPDict[bind.LoginName],
                    Socket = client
                };
                if (spPool.Count(i => i.Socket == client) <= 0)
                {
                    spPool.Add(sp);
                }  
            }

            client.Send(resp.GetBytes());

            //output
            Console.WriteLine(title + client.LocalIPAddress + " Send <Bind> ：\n" + bind.ToString());
            Console.WriteLine(title + " Send <Bind_Resp> ：\n" + resp.ToString());
        }

        private void UnBindResp(TcpSocketClient client, UnBind ubind)
        {
            var resp = new UnBind_Resp()
            {
                SequenceNumber = ubind.SequenceNumber
            };

            client.Send(resp.GetBytes());

            //output
            Console.WriteLine(title + client.LocalIPAddress + " Send <UnBind> ：\n" + ubind.ToString());
            Console.WriteLine(title + " Send <UnBindResp> ：\n" + resp.ToString());
        }

        private void SubmitResp(TcpSocketClient client, Submit submit)
        {
            var resp = new Submit_Resp
            {
                SequenceNumber = submit.SequenceNumber,
                Result = CommandError.Success
            };

            //转发给手机
            var mt = mtPool.FirstOrDefault(i => i.UserNumber.Equals(submit.UserNumber));
            if (mt != null)
            {
                mt.Socket.Send(new Deliver
                {
                    SequenceNumber = Sequence.Next(),
                    UserNumber = submit.UserNumber,
                    SPNumber = submit.SPNumber,
                    TP_pid = submit.TP_pid,
                    TP_udhi = submit.TP_udhi,
                    MessageCoding = submit.MessageCoding,
                    MessageContent = submit.MessageContent
                }.GetBytes());
            }
            
            //todo message Verify
            client.Send(resp.GetBytes());

            //output
            Console.WriteLine(title + client.LocalIPAddress + " Send <Submit> ：\n" + submit.ToString());
            Console.WriteLine(title + " Send <SubmitResp> ：\n" + resp.ToString());
        }

        private void DeliverResp(TcpSocketClient client, Deliver deliver)
        {
            var resp = new Submit_Resp
            {
                SequenceNumber = deliver.SequenceNumber,
                Result = CommandError.Success
            };

            //todo message Verify
            client.Send(resp.GetBytes());

            //output
            Console.WriteLine(title + client.LocalIPAddress + " Send <Deliver> ：\n" + deliver.ToString());
            Console.WriteLine(title + " Send <DeliverResp> ：\n" + resp.ToString());
        }

        #endregion


        public void OnRead(TcpSocketClient client, byte[] buffers)
        {
            Console.Write("\n");
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
                        break;
                    case Commands.Deliver:
                        var deliver = new Deliver(buffers);
                        this.DeliverResp(client, deliver);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                if (e is BadCmdHeadException)
                {
                    Console.WriteLine(title + client.LocalIPAddress + " 发送错误的消息头格式！");
                }
                else if (e is BadCmdBodyException)
                {
                    Console.WriteLine(title + client.LocalIPAddress + " 发送错误的消息体格式！");
                }
                else
                {
                    Console.WriteLine(title + client.LocalIPAddress + " 发送消息错误 ： " + e.Message);
                }

                //断开连接
                client.Disconnect();
            }
            finally
            {
                Console.WriteLine(">");
            }
        }
    }
}
