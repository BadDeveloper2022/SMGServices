using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Base
{
    public class BadCmdHeadException : Exception
    {
        
    }

    public class BadCmdBodyException : Exception
    {
        public uint Cmd { get; private set; }

        public BadCmdBodyException()
        {

        }

        public BadCmdBodyException(uint cmd)
        {
            this.Cmd = cmd;
        }
    }
}
