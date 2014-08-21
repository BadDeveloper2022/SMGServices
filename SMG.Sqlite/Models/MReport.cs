using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Sqlite.Models
{
    public class MReport
    {
        public int ReportID { get; set; }

        public string SubmitSequenceNumber { get; set; }

        public byte[] TargetSubmitSequenceNumber { get; set; }

        public int ReportType { get; set; }

        public string SPNumber { get; set; }

        public string UserNumber { get; set; }

        public int State { get; set; }

        public int ErrorCode { get; set; }

        /// <summary>
        /// 0、等待发送 1、已发送
        /// </summary>
        public int Status { get; set; }

        public DateTime Created { get; set; }
    }
}
