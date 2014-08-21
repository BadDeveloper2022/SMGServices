using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Sqlite.Models
{
    public class MDeliver
    {
        public int DeliverID { get; set; }

        public byte[] TargetSequenceNumber { get; set; }

        public string SequenceNumber { get; set; }

        public string UserNumber { get; set; }

        public string SPNumber { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 0、等待发送 1、已发送
        /// </summary>
        public int Status { get; set; }

        public DateTime Created { get; set; }
    }
}
