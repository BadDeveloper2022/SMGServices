using SMG.SGIP.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.SGIP.Command
{
    /// <summary>
    /// UnBind_Resp操作，无消息体
    /// </summary>
    public class UnBind_Resp : BaseCommand
    {
        public UnBind_Resp()
        {
            base.Command = Commands.UnBind_Resp;
        }

        public UnBind_Resp(byte[] bytes)
            : base(bytes)
        {
        }

    }
}
