using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Socket
{

    public delegate void ConnectedEventHandler(TcpSocketClient client);

    public delegate void RecvEventHandler(TcpSocketClient client, byte[] buffers);

}
