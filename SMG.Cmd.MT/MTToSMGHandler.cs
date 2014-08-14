using SMG.SGIP.Base;
using SMG.SGIP.Command;
using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Cmd.MT
{
    public class MTToSMGHandler
    {

        private TcpSocketClient tcpClient;
        private string title;

        public void Register(TcpSocketClient tcpClient)
        {
            this.tcpClient = tcpClient;
            tcpClient.OnConnected += this.OnConnected;
            tcpClient.OnDisconnected += this.OnDisconnected;
            tcpClient.OnRead += this.OnRead;
            tcpClient.OnSend += this.OnSend;
        }

        public void OnConnected(TcpSocketClient client)
        {
            client.Start();
            this.title = client.LocalIPAddress + ">";

            client.Send(new Bind
            {
                LoginName = "8613020320822",
                LoginPassword = "8613020320822",
                LoginType = LoginTypes.Test
            }.GetBytes());
            Console.WriteLine(title + "已连接到服务器 " + client.RemoteIPAddress);
        }

        public void OnDisconnected(TcpSocketClient client)
        {
            Console.WriteLine(title + "已从服务器 " + client.RemoteIPAddress + " 断开连接！");
        }

        public void OnSend(TcpSocketClient client, byte[] buffers)
        {

        }

        public void OnRead(TcpSocketClient client, byte[] buffers)
        {
            try
            {
                var cmd = new BaseCommand(buffers);

                switch (cmd.Command)
                {
                    case Commands.Bind_Resp:
                        var bindresp = new Bind_Resp(buffers);
                        Console.WriteLine(title + "\nServer Send <Bind_Resp> ：\n" + bindresp.ToString());
                        break;
                    case Commands.Deliver:
                        var deliver = new Deliver(buffers);
                        client.Send(new Deliver_Resp
                        {
                            SequenceNumber = deliver.SequenceNumber,
                            Result = 0
                        }.GetBytes());
                        Console.WriteLine(title + "\nServer Send <Deliver> ：\n" + deliver.ToString());
                        break;
                    case Commands.Deliver_Resp:
                        var deliver_resp = new Deliver_Resp(buffers);
                        Console.WriteLine(title + "\nServer Send <Deliver_Resp> ：\n" + deliver_resp.ToString());
                        break;
                    default:
                        break;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine(">");
            }
        }

    }
}
