using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.TcpSocket
{
    public delegate void StartEventHandler(TcpSocketServer server);

    public delegate void StopEventHandler(TcpSocketServer server);

    public delegate void ConnectedEventHandler(TcpSocketClient client);

    public delegate void DisconnectedEventHandler(TcpSocketClient client);

    public delegate void SendEventHandler(TcpSocketClient client, byte[] buffers);

    public delegate void ReadEventHandler(TcpSocketClient client, byte[] buffers);

}
