namespace KeywaySoft.Public.SGIP.DataBase
{
    using System;
    using System.Data.SqlClient;

    public class DBClass
    {
        private string _connStr;

        public DBClass(string serverName, int serverPort, string dbName, string users, string pwd)
        {
            // TODO: screen
            // this._connStr = string.Format("server={0},{1};database={2};uid={3};pwd={4}", new object[] { serverName, serverPort, dbName, users, pwd });
            this._connStr = string.Format("server={0};database={2};uid={3};pwd={4}", new object[] { serverName, serverPort, dbName, users, pwd });
        }

        public bool CheckSQLConnect()
        {
            using (SqlConnection connection = this.CreateConn())
            {
                bool flag = true;
                if (connection == null)
                {
                    flag = false;
                }
                return flag;
            }
        }

        private SqlConnection CreateConn()
        {
            try
            {
                SqlConnection connection = new SqlConnection(this._connStr);
                connection.Open();
                return connection;
            }
            catch (SqlException)
            {
                return null;
            }
        }

        public bool ExeSQL(string sql)
        {
            using (SqlConnection connection = this.CreateConn())
            {

                SqlCommand command = new SqlCommand(sql, connection);
                return (command.ExecuteNonQuery() > 0);
            }
        }

        public object LendReader(string sql, BorrowReader reader)
        {
            using (SqlConnection connection = this.CreateConn())
            {
                try
                {
                    SqlDataReader dr = new SqlCommand(sql, connection).ExecuteReader();
                    return reader(dr);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}

