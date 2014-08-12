using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Cmd.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpSocketClient client = new TcpSocketClient("127.0.0.1", 9001);
            client.Connect();
            client.Start();
            
        }
    }
}
