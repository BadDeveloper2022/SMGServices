using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    /// <summary>
    /// UnBind操作，无消息体
    /// </summary>
    public class UnBind : BaseCommand
    {
        public UnBind()
        {
            base.Command = Commands.UnBind;
            base.SequenceNumber = Sequence.Next();
        }

        public UnBind(byte[] bytes)
            : base(bytes)
        {

        }
    }
}
