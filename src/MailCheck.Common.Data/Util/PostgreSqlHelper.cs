using System;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace MailCheck.Common.Data.Util
{
    public sealed class PostgreSqlHelper
    {
        private static string stringOfBackslashChars = "\\¥Š₩∖﹨＼";
        private static string stringOfQuoteChars = "\"'`´ʹʺʻʼˈˊˋ˙̀́‘’‚′‵❛❜＇";

        private PostgreSqlHelper()
        {
        }

        /// <summary>
        /// Executes a single command against a PostgreSQL database.  The <see cref="T:PostgreSql.Data.PostgreSqlClient.NpgsqlConnection" /> is assumed to be
        /// open when the method is called and remains open after the method completes.
        /// </summary>
        /// <param name="connection"><see cref="T:PostgreSql.Data.PostgreSqlClient.NpgsqlConnection" /> object to use</param>
        /// <param name="commandText">SQL command to be executed</param>
        /// <param name="commandParameters">Array of <see cref="T:PostgreSql.Data.PostgreSqlClient.NpgsqlParameter" /> objects to use with the command.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(NpgsqlConnection connection, string commandText,
            params NpgsqlParameter[] commandParameters)
        {
            using (NpgsqlCommand postgreSqlCommand = new NpgsqlCommand())
            {
                postgreSqlCommand.Connection = connection;
                postgreSqlCommand.CommandText = commandText;
                postgreSqlCommand.CommandType = CommandType.Text;
                if (commandParameters != null)
                {
                    foreach (NpgsqlParameter commandParameter in commandParameters)
                        postgreSqlCommand.Parameters.Add(commandParameter);
                }

                int num = postgreSqlCommand.ExecuteNonQuery();
                postgreSqlCommand.Parameters.Clear();

                return num;
            }
        }

        /// <summary>
        /// Executes a single command against a PostgreSQL database.  A new <see cref="T:PostgreSql.Data.PostgreSqlClient.NpgsqlConnection" /> is created
        /// using the <see cref="P:PostgreSql.Data.PostgreSqlClient.NpgsqlConnection.ConnectionString" /> given.
        /// </summary>
        /// <param name="connectionString"><see cref="P:PostgreSql.Data.PostgreSqlClient.NpgsqlConnection.ConnectionString" /> to use</param>
        /// <param name="commandText">SQL command to be executed</param>
        /// <param name="parms">Array of <see cref="T:PostgreSql.Data.PostgreSqlClient.NpgsqlParameter" /> objects to use with the command.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, string commandText, params NpgsqlParameter[] parms)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                return PostgreSqlHelper.ExecuteNonQuery(connection, commandText, parms);
            }
        }

        /// <summary>
        /// Executes a single command against a PostgreSQL database, possibly inside an existing transaction.
        /// </summary>
        /// <param name="connection"><see cref="T:PostgreSql.Data.PostgreSqlClient.NpgsqlConnection" /> object to use for the command</param>
        /// <param name="transaction"><see cref="T:PostgreSql.Data.PostgreSqlClient.NpgsqlTransaction" /> object to use for the command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="T:PostgreSql.Data.PostgreSqlClient.NpgsqlParameter" /> objects to use with the command</param>
        /// <param name="externalConn">True if the connection should be preserved, false if not</param>
        /// <returns><see cref="T:PostgreSql.Data.PostgreSqlClient.PostgreSqlDataReader" /> object ready to read the results of the command</returns>
        private static DbDataReader ExecuteReader(NpgsqlConnection connection, NpgsqlTransaction transaction,
            string commandText, NpgsqlParameter[] commandParameters, bool externalConn)
        {
            using (NpgsqlCommand postgreSqlCommand = new NpgsqlCommand())
            {
                postgreSqlCommand.Connection = connection;
                postgreSqlCommand.Transaction = transaction;
                postgreSqlCommand.CommandText = commandText;
                postgreSqlCommand.CommandType = CommandType.Text;
                if (commandParameters != null)
                {
                    foreach (NpgsqlParameter commandParameter in commandParameters)
                        postgreSqlCommand.Parameters.Add(commandParameter);
                }

                DbDataReader sqlDataReader = !externalConn
                    ? postgreSqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                    : postgreSqlCommand.ExecuteReader();
                postgreSqlCommand.Parameters.Clear();
                return sqlDataReader;
            }
        }

        /// <summary>Executes a single command against a PostgreSQL database.</summary>
        /// <param name="connectionString">Settings to use for this command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="T:PostgreSql.Data.PostgreSqlClient.NpgsqlParameter" /> objects to use with the command</param>
        /// <returns><see cref="T:PostgreSql.Data.PostgreSqlClient.PostgreSqlDataReader" /> object ready to read the results of the command</returns>
        public static DbDataReader ExecuteReader(string connectionString, string commandText,
            params NpgsqlParameter[] commandParameters)
        {
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            string commandText1 = commandText;
            NpgsqlParameter[] commandParameters1 = commandParameters;
            int num = 0;
            return PostgreSqlHelper.ExecuteReader(connection, (NpgsqlTransaction)null, commandText1, commandParameters1,
                num != 0);
        }

        /// <summary>Async version of ExecuteNonQuery</summary>
        /// <param name="connectionString"><see cref="P:PostgreSql.Data.PostgreSqlClient.NpgsqlConnection.ConnectionString" /> to use</param>
        /// <param name="commandText">SQL command to be executed</param>
        /// <param name="commandParameters">Array of <see cref="T:PostgreSql.Data.PostgreSqlClient.NpgsqlParameter" /> objects to use with the command.</param>
        /// <returns>Rows affected</returns>
        public static Task<int> ExecuteNonQueryAsync(string connectionString, string commandText,
            params NpgsqlParameter[] commandParameters)
        {
            return PostgreSqlHelper.ExecuteNonQueryAsync(connectionString, commandText, CancellationToken.None,
                commandParameters);
        }

        public static Task<int> ExecuteNonQueryAsync(string connectionString, string commandText,
            CancellationToken cancellationToken, params NpgsqlParameter[] commandParameters)
        {
            TaskCompletionSource<int> completionSource = new TaskCompletionSource<int>();
            if (cancellationToken == CancellationToken.None || !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    int result = PostgreSqlHelper.ExecuteNonQuery(connectionString, commandText, commandParameters);
                    completionSource.SetResult(result);
                }
                catch (Exception ex)
                {
                    completionSource.SetException(ex);
                }
            }
            else
                completionSource.SetCanceled();

            return completionSource.Task;
        }

        /// <summary>Async version of ExecuteReader</summary>
        /// <param name="connectionString">Settings to use for this command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="T:PostgreSql.Data.PostgreSqlClient.NpgsqlParameter" /> objects to use with the command</param>
        /// <returns><see cref="T:PostgreSql.Data.PostgreSqlClient.PostgreSqlDataReader" /> object ready to read the results of the command</returns>
        public static Task<DbDataReader> ExecuteReaderAsync(string connectionString, string commandText,
            params NpgsqlParameter[] commandParameters)
        {
            return PostgreSqlHelper.ExecuteReaderAsync(connectionString, commandText, CancellationToken.None,
                commandParameters);
        }

        public static Task<DbDataReader> ExecuteReaderAsync(string connectionString, string commandText,
            CancellationToken cancellationToken, params NpgsqlParameter[] commandParameters)
        {
            TaskCompletionSource<DbDataReader> completionSource = new TaskCompletionSource<DbDataReader>();
            if (cancellationToken == CancellationToken.None || !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    DbDataReader result = PostgreSqlHelper.ExecuteReader(connectionString, commandText, commandParameters);
                    completionSource.SetResult(result);
                }
                catch (Exception ex)
                {
                    completionSource.SetException(ex);
                }
            }
            else
                completionSource.SetCanceled();

            return completionSource.Task;
        }
    }
}