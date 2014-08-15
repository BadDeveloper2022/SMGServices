using SMG.SGIP.Base;
using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SMG.Cmd.SMG
{
    class Program
    {
        static TcpSocketServer SMGToSPServer;
        static TcpSocketServer SMGToMTServer;
        static List<SPClient> spPool;
        static List<MTClient> mtPool;

        static void Bind(string ip, int port)
        {
            if (SMGToSPServer == null || !SMGToSPServer.Listened)
            {
                SMGToSPServer = new TcpSocketServer(ip, port);
                SMGToSPHandler handler = new SMGToSPHandler(spPool, mtPool);
                handler.Register(SMGToSPServer);
                SMGToSPServer.Listen(20);
            }

            if (SMGToMTServer == null || !SMGToMTServer.Listened)
            {
                SMGToMTServer = new TcpSocketServer(ip, port + 1);
                SMGToMTHandler handler = new SMGToMTHandler(spPool, mtPool);
                handler.Register(SMGToMTServer);
                SMGToMTServer.Listen(20);                
            }
        }

        static void UnBind()
        {
            if (SMGToSPServer != null)
            {
                SMGToSPServer.Stop();
            }

            if (SMGToMTServer == null || SMGToMTServer.Listened)
            {
                SMGToMTServer.Stop();
            }
        }

        static void Main(string[] args)
        {
            Sequence.SetNodeNumber(123456);
            spPool = new List<SPClient>();
            mtPool = new List<MTClient>();

            Console.Write(">");
            string cmd = Console.ReadLine();

            while (cmd != "quit")
            {
                if (cmd != "")
                {
                    if (cmd.StartsWith("bind"))
                    {
                        try
                        {
                            string[] param = cmd.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            string ip = "";
                            int port = 0;
                            if (param.Length <= 1)
                            {
                                ip = "127.0.0.1";
                                port = 8801;
                            }
                            else
                            {
                                ip = param[1];
                                port = int.Parse(param[2]);
                            }

                            Bind(ip, port);
                        }
                        catch
                        {
                            Console.WriteLine("invalid 'bind' parameters ");
                        }
                    }
                    else
                    {
                        switch (cmd)
                        {
                            case "clear":
                                Console.Clear();
                                break;
                            case "unbind":
                                UnBind();
                                break;
                            case "ipconfig":
                                IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
                                ipEntry.AddressList.Where(i => !i.IsIPv6LinkLocal).ToList().ForEach(i => Console.WriteLine(i));
                                break;
                            default:
                                Console.WriteLine(cmd);
                                break;
                        }
                    }
                }

                Console.Write(">");
                cmd = Console.ReadLine();
            }
        }
    }
}
