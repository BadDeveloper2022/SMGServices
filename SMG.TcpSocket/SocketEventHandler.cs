using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.TcpSocket
{
    public delegate void StopEventHandler();

    public delegate void ConnectedEventHandler(TcpSocketClient client);

    public delegate void DisconnectedEventHandler(TcpSocketClient client);

    public delegate void SendEventHandler(TcpSocketClient client, byte[] buffers);

    public delegate void ReadEventHandler(TcpSocketClient client, byte[] buffers);

}
