using SMG.Sqlite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMG.Sqlite.Storage
{
    public class DeliverStorage : BaseStorage
    {
        internal DeliverStorage()
        {

        }

        public int Insert(MDeliver m)
        {
            int count = 0;
            string sql = @"INSERT INTO Deliver (TargetSequenceNumber , SequenceNumber ,UserNumber , SPNumber , Content , Status , Created)
                                VALUES(@TargetSequenceNumber , @SequenceNumber , @UserNumber , @SPNumber , @Content , @Status , @Created)";

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
            string sql = @"UPDATE Deliver SET Status = @Status WHERE SequenceNumber = @SequenceNumber";

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

        public IEnumerable<MDeliver> GetList(string spNumber)
        {
            IEnumerable<MDeliver> ls = null;

            string sql = "SELECT * FROM Deliver WHERE SPNumber = @SPNumber AND Status = 0 ";

            try
            {
                ls = base.Query<MDeliver>(sql, new { SPNumber = spNumber });
            }
            catch
            {
                throw;
            }

            return ls;
        }

    }
}
