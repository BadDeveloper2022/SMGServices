using SMG.Sqlite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Sqlite.Storage
{
    public class ReportStorage : BaseStorage
    {
        internal ReportStorage()
        {

        }

        public int Insert(MReport m)
        {
            int count = 0;
            string sql = @"INSERT INTO Report (TargetSubmitSequenceNumber , SubmitSequenceNumber ,ReportType , SPNumber , UserNumber , State , Status , ErrorCode , Created)
                            VALUES(@TargetSubmitSequenceNumber , @SubmitSequenceNumber ,@ReportType , @SPNumber , @UserNumber , @State , @Status , @ErrorCode , @Created)";

            try
            {
                count += base.Execute(sql, m);
            }
            catch
            {
                throw;
            }

            return count;
        }

        public MReport Get(string submitSequenceNumber)
        {
            MReport m = null;
            string sql = @"SELECT * from Report WHERE SubmitSequenceNumber = @SubmitSequenceNumber limit 0 , 1 ";

            try
            {
                m = base.Query<MReport>(sql, new { SubmitSequenceNumber = submitSequenceNumber }).FirstOrDefault();
            }
            catch
            {
                throw;
            }

            return m;
        }


        public int Update(MReport m)
        {
            int count = 0;
            string sql = @"UPDATE Report SET State = @State , ErrorCode = @ErrorCode , Status = @Status WHERE SubmitSequenceNumber = @SubmitSequenceNumber";

            try
            {
                count += base.Execute(sql, m);
            }
            catch
            {
                throw;
            }

            return count;
        }

        public IEnumerable<MReport> GetList(string spNumber)
        {
            IEnumerable<MReport> ls = null;

            string sql = "SELECT * FROM Report WHERE SPNumber = @SPNumber AND Status = 0 ";

            try
            {
                ls = base.Query<MReport>(sql, new { SPNumber = spNumber });
            }
            catch
            {
                throw;
            }

            return ls;
        }

    }
}
