using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.CmdApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpSocketServer tcpServer = new TcpSocketServer("127.0.0.1", 9001);
            tcpServer.OnRecv += SocketClientEventsHandle.OnRecv;

            tcpServer.Listen(50);

            Console.WriteLine(tcpServer.BindIPAddress + "> 网关服务已启动！");
            Console.Write(tcpServer.BindIPAddress + ">");
            string cmd = Console.ReadLine();

            while (cmd != "quit")
            {
                if (cmd != "")
                {
                    Console.WriteLine(cmd);
                }          
                Console.Write(tcpServer.BindIPAddress + ">");
                cmd = Console.ReadLine();
            }
        }
    }
}
