using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emulator.Phone.Utils
{
    public enum SMSTypes
    {
        SEND,
        RECEIVE
    }

    public class SMS
    {
        public string UserNumber { get; set; }

        public string SPNumber { get; set; }

        public string Content { get; set; }

        public SMSTypes Type { get; set; }

        public DateTime Time { get; set; }
    }
}
