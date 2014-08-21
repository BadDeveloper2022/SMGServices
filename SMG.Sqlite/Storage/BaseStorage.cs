using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;
using System.Data.SQLite;

namespace SMG.Sqlite
{
    public class BaseStorage
    {
        private ConnectionStringSettings connectionStringSettings;

        internal BaseStorage()
        {
            try
            {
                this.connectionStringSettings = ConfigurationManager.ConnectionStrings["SMGDB"];
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 获取数据库连接(connection will auto close when finish query)
        /// </summary>
        /// <returns></returns>
        protected IDbConnection GetDbConnection()
        {
            IDbConnection connection = null;

            if (connectionStringSettings.ProviderName.Equals("System.Data.SQLite"))
            {
                connection = new SQLiteConnection(this.connectionStringSettings.ConnectionString);
                connection.Open();
            }

            return connection;
        }

        protected IEnumerable<T> Query<T>(string sql, dynamic param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            IEnumerable<T> result = null;

            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    var conn = transaction != null ? transaction.Connection : this.GetDbConnection();
                    result = conn.Query<T>(sql, param as object, transaction, buffered, commandTimeout, commandType);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        protected IEnumerable<dynamic> Query(string sql, dynamic param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            IEnumerable<dynamic> result = null;

            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    var conn = transaction != null ? transaction.Connection : this.GetDbConnection();
                    result = conn.Query(sql, param as object, transaction, buffered, commandTimeout, commandType);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        protected SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            SqlMapper.GridReader result = null;

            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    var conn = transaction != null ? transaction.Connection : this.GetDbConnection();
                    result = conn.QueryMultiple(sql, param as object, transaction, commandTimeout, commandType);
                }
            }
            catch (Exception e)
            {
                throw e;
            }


            return result;
        }

        protected int Execute(string sql, dynamic param = null, CommandType commandType = CommandType.Text, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            int result = 0;

            try
            {
                if (!string.IsNullOrEmpty(sql))
                {
                    var conn = transaction != null ? transaction.Connection : this.GetDbConnection();
                    result = conn.Execute(sql, param as object, transaction, commandTimeout, commandType);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

    }
}
