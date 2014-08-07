namespace KeywaySoft.Public.SGIP.Base
{
    using System;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    public delegate void NetEventDelegate(byte[] bs, int recvCount, Socket soc);
}

