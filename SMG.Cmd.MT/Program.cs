using SMG.SGIP.Base;
using SMG.SGIP.Command;
using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Cmd.MT
{
    class Program
    {
        static void Connect(string cmd, ref TcpSocketClient tcpClient, ref string title)
        {
            try
            {
                if (tcpClient == null || !tcpClient.Connected)
                {
                    string[] param = cmd.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    string ip = "";
                    int port = 0;
                    if (param.Length <= 1)
                    {
                        ip = "127.0.0.1";
                        port = 8802;
                    }
                    else
                    {
                        ip = param[1];
                        port = int.Parse(param[2]);
                    }

                    tcpClient = new TcpSocketClient(ip, port);
                    MTToSMGHandler mbHandler = new MTToSMGHandler();
                    mbHandler.Register(tcpClient);
                    tcpClient.Connect();
                }
            }
            catch
            {
                Console.WriteLine("invalid 'connect' parameters ");
            }
        }

        public static string UserName { get; set; }

        static void Main(string[] args)
        {
            TcpSocketClient tcpClient = null;
            string title = "";

            Console.Write(">");
            string cmd = Console.ReadLine();

            UserName = "8613020320822";

            while (cmd != "quit")
            {
                if (cmd != "")
                {
                    if (cmd.StartsWith("connect"))
                    {
                        Connect(cmd, ref tcpClient, ref title);
                    }
                    else if (cmd.StartsWith("send"))
                    {
                        if (tcpClient != null && tcpClient.Connected)
                        {
                            try
                            {
                                string[] param = cmd.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                tcpClient.Send(new Deliver
                                {
                                    SequenceNumber = Sequence.Next(),
                                    SPNumber = "106559284130016",
                                    UserNumber = "8613020320822",
                                    TP_pid = 0,
                                    TP_udhi = 0,
                                    MessageCoding = MessageCodes.GBK,
                                    MessageContent = param[1]
                                }.GetBytes());
                            }
                            catch
                            {
                                Console.WriteLine("invalid 'send' parameters ");
                            }
                        }
                    }
                    else if (cmd.StartsWith("setnumber"))
                    {
                        try
                        {
                            if (tcpClient == null || !tcpClient.Connected)
                            {
                                string[] param = cmd.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                UserName = param[1];
                            } 
                        }
                        catch
                        {
                            Console.WriteLine("invalid 'setnumber' parameters ");
                        }
                    }
                    else
                    {
                        switch (cmd)
                        {
                            case "clear":
                                Console.Clear();
                                break;
                            case "disconnect":
                                if (tcpClient != null && tcpClient.Connected)
                                {
                                    tcpClient.Disconnect();
                                }
                                break;
                            case "getnumber":
                                Console.WriteLine(UserName);
                                break;
                            default:
                                Console.WriteLine(cmd);
                                break;
                        }
                    }
                }

                Console.Write(title + ">");
                cmd = Console.ReadLine();
            }
        }
    }
}
