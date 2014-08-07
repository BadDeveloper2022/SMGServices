using SMG.SGIP.Base;
using SMG.SGIP.Command;
using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.CmdApp
{
    public class SocketClientEventsHandle
    {
        public static void OnRecv(TcpSocketClient client, byte[] buffers)
        {
            if (buffers.Length > 20)
            {
                BaseCommand cmd = new BaseCommand(buffers);
                switch (cmd.Command)
                {
                    case Commands.Bind:
                        var bind = new Bind(buffers);
                        Console.WriteLine("logintype : " + bind.LoginType + " , username : " + bind.UserName + " , password : " + bind.Password);
                        var resp = new BindResp()
                        {
                            SequenceNumber = bind.SequenceNumber,
                            Result = 0
                        };

                        byte[] data = resp.GetBytes();
                        client.Send(data);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine("消息不合法");
            }


        }

    }
}
