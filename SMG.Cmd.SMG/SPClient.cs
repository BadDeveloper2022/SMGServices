﻿using SMG.TcpSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Cmd.SMG
{
    public class SPClient
    {
        public string SPNumber { get; set; }

        public TcpSocketClient Socket { get; set; }
    }
}
