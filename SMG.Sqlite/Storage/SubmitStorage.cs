using SMG.Sqlite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Sqlite.Storage
{
    public class SubmitStorage : BaseStorage
    {
        internal SubmitStorage()
        {

        }


        public int Insert(MSubmit m)
        {
            int count = 0;
            string sql = @"INSERT INTO Submit (TargetSequenceNumber , SequenceNumber , SPNumber , UserNumber , ReportFlag , Content , Status , Created)
                            VALUES(@TargetSequenceNumber , @SequenceNumber , @SPNumber , @UserNumber , @ReportFlag , @Content , @Status , @Created)";

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

        public int Update(string sequenceNumber, int status)
        {
            int count = 0;
            string sql = @"UPDATE Submit SET Status = @Status WHERE SequenceNumber = @SequenceNumber";

            try
            {
                count += base.Execute(sql, new { Status = status, SequenceNumber = sequenceNumber });
            }
            catch
            {
                throw;
            }

            return count;
        }

        public IEnumerable<MSubmit> GetList(string userNumber)
        {
            IEnumerable<MSubmit> ls = null;

            string sql = "SELECT * FROM Submit WHERE UserNumber = @UserNumber AND Status = 0 ";

            try
            {
                ls = base.Query<MSubmit>(sql, new { UserNumber = userNumber });
            }
            catch
            {
                throw;
            }

            return ls;
        }

    }
}
