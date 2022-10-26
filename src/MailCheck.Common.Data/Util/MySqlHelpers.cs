using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MailCheck.Common.Data.Util
{
    public static class MySqlHelper
    {
        private static string stringOfBackslashChars = "\\¥Š₩∖﹨＼";
        private static string stringOfQuoteChars = "\"'`´ʹʺʻʼˈˊˋ˙̀́‘’‚′‵❛❜＇";
        private static MySqlHelper.CharClass[] charClassArray = MySqlHelper.MakeCharClassArray();

        /// <summary>
        /// Execute a command which returns no value against a MySQL database. The <see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> is assumed to be
        /// open when the method is called and remains open after the method completes.
        /// </summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use</param>
        /// <param name="commandText">SQL command to be executed</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(MySqlConnection connection, string commandText, params MySqlParameter[] commandParameters)
        {
            return ExecuteNonQueryImpl(connection, null, commandText, commandParameters);
        }

        /// <summary>
        /// Execute a command which returns no value against a MySQL database. The <see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> is assumed to be
        /// open when the method is called and remains open after the method completes.
        /// </summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use</param>
        /// <param name="transaction">Transaction to attach execute query within</param>
        /// <param name="commandText">SQL command to be executed</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(MySqlConnection connection, MySqlTransaction transaction, string commandText, params MySqlParameter[] commandParameters)
        {
            return ExecuteNonQueryImpl(connection, transaction, commandText, commandParameters);
        }

        /// <summary>
        /// Execute a command which returns no value against a MySQL database. A new <see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> is created
        /// using the <see cref="P:MySql.Data.MySqlClient.MySqlConnection.ConnectionString" /> given.
        /// </summary>
        /// <param name="connectionString"><see cref="P:MySql.Data.MySqlClient.MySqlConnection.ConnectionString" /> to use</param>
        /// <param name="commandText">SQL command to be executed</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, string commandText, params MySqlParameter[] commandParameters)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteNonQueryImpl(connection, null, commandText, commandParameters);
            }
        }

        /// <summary>Execute a command which returns records against a MySQL database.</summary>
        /// <param name="connectionString">Settings to use for this command</param>
        /// <param name="commandText">Command text to use</param>
        /// <returns><see cref="T:MySql.Data.MySqlClient.MySqlDataReader" /> object ready to read the results of the command</returns>
        public static DbDataReader ExecuteReader(string connectionString, string commandText)
        {
            return MySqlHelper.ExecuteReader(connectionString, commandText, (MySqlParameter[]) null);
        }

        /// <summary>Execute a command which returns records against a MySQL database.</summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use for the command</param>
        /// <param name="commandText">Command text to use</param>
        /// <returns><see cref="T:MySql.Data.MySqlClient.MySqlDataReader" /> object ready to read the results of the command</returns>
        public static DbDataReader ExecuteReader(MySqlConnection connection, string commandText)
        {
            return MySqlHelper.ExecuteReaderImpl(connection, (MySqlTransaction) null, commandText, (MySqlParameter[]) null,
                true);
        }

        /// <summary>Execute a command which returns records against a MySQL database.</summary>
        /// <param name="connectionString">Settings to use for this command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command</param>
        /// <returns><see cref="T:MySql.Data.MySqlClient.MySqlDataReader" /> object ready to read the results of the command</returns>
        public static DbDataReader ExecuteReader(string connectionString, string commandText,
            params MySqlParameter[] commandParameters)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            return MySqlHelper.ExecuteReaderImpl(connection, (MySqlTransaction) null, commandText, commandParameters, false);
        }

        /// <summary>Execute a command which returns records against a MySQL database.</summary>
        /// <param name="connection">Connection to use for the command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command</param>
        /// <returns><see cref="T:MySql.Data.MySqlClient.MySqlDataReader" /> object ready to read the results of the command</returns>
        public static DbDataReader ExecuteReader(MySqlConnection connection, string commandText,
            params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteReaderImpl(connection, (MySqlTransaction) null, commandText, commandParameters, true);
        }

        /// <summary>Execute a command which returns records against a MySQL database.</summary>
        /// <param name="connection">Connection to use for the command</param>
        /// <param name="transaction"></param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command</param>
        /// <returns><see cref="T:MySql.Data.MySqlClient.MySqlDataReader" /> object ready to read the results of the command</returns>
        public static DbDataReader ExecuteReader(MySqlConnection connection, MySqlTransaction transaction, string commandText,
            params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteReaderImpl(connection, transaction, commandText, commandParameters, true);
        }

        /// <summary>Execute a command which returns a single value against a MySQL database.</summary>
        /// <param name="connectionString">Settings to use for the update</param>
        /// <param name="commandText">Command text to use for the update</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(string connectionString, string commandText)
        {
            return MySqlHelper.ExecuteScalar(connectionString, commandText, (MySqlParameter[]) null);
        }

        /// <summary>Execute a command which returns a single value against a MySQL database.</summary>
        /// <param name="connectionString">Settings to use for the command</param>
        /// <param name="commandText">Command text to use for the command</param>
        /// <param name="commandParameters">Parameters to use for the command</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(string connectionString, string commandText,
            params MySqlParameter[] commandParameters)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                return MySqlHelper.ExecuteScalar(connection, commandText, commandParameters);
            }
        }

        /// <summary>Execute a command which returns a single value against a MySQL database.</summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use</param>
        /// <param name="commandText">Command text to use for the command</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(MySqlConnection connection, string commandText)
        {
            return MySqlHelper.ExecuteScalar(connection, commandText, (MySqlParameter[]) null);
        }

        /// <summary>Execute a command which returns a single value against a MySQL database.</summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use</param>
        /// <param name="commandText">Command text to use for the command</param>
        /// <param name="commandParameters">Parameters to use for the command</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(MySqlConnection connection, string commandText,
            params MySqlParameter[] commandParameters)
        {
            return ExecuteScalarImpl(connection, null, commandText, commandParameters);
        }

        /// <summary>Execute a command which returns a single value against a MySQL database.</summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use</param>
        /// <param name="transaction"></param>
        /// <param name="commandText">Command text to use for the command</param>
        /// <param name="commandParameters">Parameters to use for the command</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static object ExecuteScalar(MySqlConnection connection, MySqlTransaction transaction, string commandText,
            params MySqlParameter[] commandParameters)
        {
            return ExecuteScalarImpl(connection, transaction, commandText, commandParameters);
        }

        private static MySqlHelper.CharClass[] MakeCharClassArray()
        {
            MySqlHelper.CharClass[] charClassArray = new MySqlHelper.CharClass[65536];
            foreach (char stringOfBackslashChar in MySqlHelper.stringOfBackslashChars)
                charClassArray[(int) stringOfBackslashChar] = MySqlHelper.CharClass.Backslash;
            foreach (char stringOfQuoteChar in MySqlHelper.stringOfQuoteChars)
                charClassArray[(int) stringOfQuoteChar] = MySqlHelper.CharClass.Quote;
            return charClassArray;
        }

        private static bool NeedsQuoting(string s)
        {
            foreach (char c in s)
            {
                if (charClassArray[c] != CharClass.None)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Escapes the string.</summary>
        /// <param name="value">The string to escape</param>
        /// <returns>The string with all quotes escaped.</returns>
        public static string EscapeString(string value)
        {
            if (!MySqlHelper.NeedsQuoting(value))
                return value;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char ch in value)
            {
                if (MySqlHelper.charClassArray[(int) ch] != MySqlHelper.CharClass.None)
                    stringBuilder.Append("\\");
                stringBuilder.Append(ch);
            }

            return stringBuilder.ToString();
        }

        public static string DoubleQuoteString(string value)
        {
            if (!MySqlHelper.NeedsQuoting(value))
                return value;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char ch in value)
            {
                switch (MySqlHelper.charClassArray[(int) ch])
                {
                    case MySqlHelper.CharClass.Quote:
                        stringBuilder.Append(ch);
                        break;
                    case MySqlHelper.CharClass.Backslash:
                        stringBuilder.Append("\\");
                        break;
                }

                stringBuilder.Append(ch);
            }

            return stringBuilder.ToString();
        }

        /// <summary>Async version of ExecuteNonQuery</summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use</param>
        /// <param name="commandText">SQL command to be executed</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command.</param>
        /// <returns>Rows affected</returns>
        public static Task<int> ExecuteNonQueryAsync(MySqlConnection connection, string commandText,
            params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteNonQueryAsync(connection, null, commandText, CancellationToken.None, commandParameters);
        }

        /// <summary>
        /// Executes a single command against a MySQL database.  The <see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> is assumed to be
        /// open when the method is called and remains open after the method completes.
        /// </summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use</param>
        /// <param name="transaction">Transaction to attach execute query within</param>
        /// <param name="commandText">SQL command to be executed</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command.</param>
        /// <returns></returns>
        public static Task<int> ExecuteNonQueryAsync(MySqlConnection connection, MySqlTransaction transaction, string commandText, params MySqlParameter[] commandParameters)
        {
            return ExecuteNonQueryAsync(connection, transaction, commandText, CancellationToken.None, commandParameters);
        }

        public static Task<int> ExecuteNonQueryAsync(MySqlConnection connection, string commandText,
            CancellationToken cancellationToken, params MySqlParameter[] commandParameters)
        {
            return ExecuteNonQueryAsync(connection, null, commandText, cancellationToken, commandParameters);
        }

        public static Task<int> ExecuteNonQueryAsync(MySqlConnection connection, MySqlTransaction transaction, string commandText,
            CancellationToken cancellationToken, params MySqlParameter[] commandParameters)
        {
            TaskCompletionSource<int> completionSource = new TaskCompletionSource<int>();
            if (cancellationToken == CancellationToken.None || !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    int result = ExecuteNonQueryImpl(connection, transaction, commandText, commandParameters);
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

        /// <summary>Async version of ExecuteNonQuery</summary>
        /// <param name="connectionString"><see cref="P:MySql.Data.MySqlClient.MySqlConnection.ConnectionString" /> to use</param>
        /// <param name="commandText">SQL command to be executed</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command.</param>
        /// <returns>Rows affected</returns>
        public static Task<int> ExecuteNonQueryAsync(string connectionString, string commandText,
            params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteNonQueryAsync(connectionString, commandText, CancellationToken.None,
                commandParameters);
        }

        public static Task<int> ExecuteNonQueryAsync(string connectionString, string commandText,
            CancellationToken cancellationToken, params MySqlParameter[] commandParameters)
        {
            TaskCompletionSource<int> completionSource = new TaskCompletionSource<int>();
            if (cancellationToken == CancellationToken.None || !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    int result = MySqlHelper.ExecuteNonQuery(connectionString, commandText, commandParameters);
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

        private static Task<DbDataReader> ExecuteReaderAsync(MySqlConnection connection, MySqlTransaction transaction,
            string commandText, MySqlParameter[] commandParameters, bool externalConn,
            CancellationToken cancellationToken)
        {
            TaskCompletionSource<DbDataReader> completionSource = new TaskCompletionSource<DbDataReader>();
            if (cancellationToken == CancellationToken.None || !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    DbDataReader result = MySqlHelper.ExecuteReaderImpl(connection, transaction, commandText,
                        commandParameters, externalConn);
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
        /// <returns><see cref="T:MySql.Data.MySqlClient.MySqlDataReader" /> object ready to read the results of the command</returns>
        public static Task<DbDataReader> ExecuteReaderAsync(string connectionString, string commandText)
        {
            return MySqlHelper.ExecuteReaderAsync(connectionString, commandText, CancellationToken.None,
                (MySqlParameter[]) null);
        }

        public static Task<DbDataReader> ExecuteReaderAsync(string connectionString, string commandText,
            CancellationToken cancellationToken)
        {
            return MySqlHelper.ExecuteReaderAsync(connectionString, commandText, cancellationToken,
                (MySqlParameter[]) null);
        }

        /// <summary>Async version of ExecuteReader</summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use for the command</param>
        /// <param name="commandText">Command text to use</param>
        /// <returns><see cref="T:MySql.Data.MySqlClient.MySqlDataReader" /> object ready to read the results of the command</returns>
        public static Task<DbDataReader> ExecuteReaderAsync(MySqlConnection connection, string commandText)
        {
            return MySqlHelper.ExecuteReaderAsync(connection, (MySqlTransaction) null, commandText,
                (MySqlParameter[]) null, true, CancellationToken.None);
        }

        public static Task<DbDataReader> ExecuteReaderAsync(MySqlConnection connection, string commandText,
            CancellationToken cancellationToken)
        {
            return MySqlHelper.ExecuteReaderAsync(connection, (MySqlTransaction) null, commandText,
                (MySqlParameter[]) null, true, cancellationToken);
        }

        /// <summary>Async version of ExecuteReader</summary>
        /// <param name="connectionString">Settings to use for this command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command</param>
        /// <returns><see cref="T:MySql.Data.MySqlClient.MySqlDataReader" /> object ready to read the results of the command</returns>
        public static Task<DbDataReader> ExecuteReaderAsync(string connectionString, string commandText,
            params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteReaderAsync(connectionString, commandText, CancellationToken.None,
                commandParameters);
        }

        public static Task<DbDataReader> ExecuteReaderAsync(string connectionString, string commandText,
            CancellationToken cancellationToken, params MySqlParameter[] commandParameters)
        {
            TaskCompletionSource<DbDataReader> completionSource = new TaskCompletionSource<DbDataReader>();
            if (cancellationToken == CancellationToken.None || !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    DbDataReader result = MySqlHelper.ExecuteReader(connectionString, commandText, commandParameters);
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
        /// <param name="connection">Connection to use for the command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command</param>
        /// <returns><see cref="T:MySql.Data.MySqlClient.MySqlDataReader" /> object ready to read the results of the command</returns>
        public static Task<DbDataReader> ExecuteReaderAsync(MySqlConnection connection, string commandText,
            params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteReaderAsync(connection, (MySqlTransaction) null, commandText, commandParameters,
                true, CancellationToken.None);
        }

        public static Task<DbDataReader> ExecuteReaderAsync(MySqlConnection connection, string commandText,
            CancellationToken cancellationToken, params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteReaderAsync(connection, (MySqlTransaction) null, commandText, commandParameters,
                true, cancellationToken);
        }

        /// <summary>Async version of ExecuteReader</summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use for the command</param>
        /// <param name="transaction"><see cref="T:MySql.Data.MySqlClient.MySqlTransaction" /> object to use for the command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command</param>
        /// <returns><see cref="T:MySql.Data.MySqlClient.MySqlDataReader" /> object ready to read the results of the command</returns>
        public static Task<DbDataReader> ExecuteReaderAsync(MySqlConnection connection, MySqlTransaction transaction,
            string commandText, params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteReaderAsync(connection, transaction, commandText, commandParameters, true,
                CancellationToken.None);
        }

        /// <summary>Async version of ExecuteReader</summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use for the command</param>
        /// <param name="transaction"><see cref="T:MySql.Data.MySqlClient.MySqlTransaction" /> object to use for the command</param>
        /// <param name="commandText">Command text to use</param>
        /// <param name="commandParameters">Array of <see cref="T:MySql.Data.MySqlClient.MySqlParameter" /> objects to use with the command</param>
        /// <param name="cancellationToken"></param>
        /// <returns><see cref="T:MySql.Data.MySqlClient.MySqlDataReader" /> object ready to read the results of the command</returns>
        public static Task<DbDataReader> ExecuteReaderAsync(MySqlConnection connection, MySqlTransaction transaction,
            string commandText, CancellationToken cancellationToken, params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteReaderAsync(connection, transaction, commandText, commandParameters, true,
                cancellationToken);
        }

        /// <summary>Async version of ExecuteScalar</summary>
        /// <param name="connectionString">Settings to use for the update</param>
        /// <param name="commandText">Command text to use for the update</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static Task<object> ExecuteScalarAsync(string connectionString, string commandText)
        {
            return MySqlHelper.ExecuteScalarAsync(connectionString, commandText, CancellationToken.None,
                (MySqlParameter[]) null);
        }

        public static Task<object> ExecuteScalarAsync(string connectionString, string commandText,
            CancellationToken cancellationToken)
        {
            return MySqlHelper.ExecuteScalarAsync(connectionString, commandText, cancellationToken,
                (MySqlParameter[]) null);
        }

        /// <summary>Async version of ExecuteScalar</summary>
        /// <param name="connectionString">Settings to use for the command</param>
        /// <param name="commandText">Command text to use for the command</param>
        /// <param name="commandParameters">Parameters to use for the command</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static Task<object> ExecuteScalarAsync(string connectionString, string commandText,
            params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteScalarAsync(connectionString, commandText, CancellationToken.None,
                commandParameters);
        }

        public static Task<object> ExecuteScalarAsync(string connectionString, string commandText,
            CancellationToken cancellationToken, params MySqlParameter[] commandParameters)
        {
            TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
            if (cancellationToken == CancellationToken.None || !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    object result = MySqlHelper.ExecuteScalar(connectionString, commandText, commandParameters);
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

        /// <summary>Async version of ExecuteScalar</summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use</param>
        /// <param name="commandText">Command text to use for the command</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static Task<object> ExecuteScalarAsync(MySqlConnection connection, string commandText)
        {
            return MySqlHelper.ExecuteScalarAsync(connection, null, commandText, CancellationToken.None,
                (MySqlParameter[]) null);
        }

        public static Task<object> ExecuteScalarAsync(MySqlConnection connection, string commandText,
            CancellationToken cancellationToken)
        {
            return MySqlHelper.ExecuteScalarAsync(connection, null, commandText, cancellationToken, (MySqlParameter[]) null);
        }

        /// <summary>Async version of ExecuteScalar</summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use</param>
        /// <param name="commandText">Command text to use for the command</param>
        /// <param name="commandParameters">Parameters to use for the command</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static Task<object> ExecuteScalarAsync(MySqlConnection connection, string commandText,
            params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteScalarAsync(connection, null, commandText, CancellationToken.None, commandParameters);
        }

        /// <summary>Async version of ExecuteScalar</summary>
        /// <param name="connection"><see cref="T:MySql.Data.MySqlClient.MySqlConnection" /> object to use</param>
        /// <param name="transaction"></param>
        /// <param name="commandText">Command text to use for the command</param>
        /// <param name="commandParameters">Parameters to use for the command</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public static Task<object> ExecuteScalarAsync(MySqlConnection connection, MySqlTransaction transaction, string commandText,
            params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteScalarAsync(connection, transaction, commandText, CancellationToken.None, commandParameters);
        }

        public static Task<object> ExecuteScalarAsync(MySqlConnection connection, string commandText,
           CancellationToken cancellationToken, params MySqlParameter[] commandParameters)
        {
            return MySqlHelper.ExecuteScalarAsync(connection, null, commandText, cancellationToken, commandParameters);
        }

        public static Task<object> ExecuteScalarAsync(MySqlConnection connection, MySqlTransaction transaction, string commandText,
            CancellationToken cancellationToken, params MySqlParameter[] commandParameters)
        {
            TaskCompletionSource<object> completionSource = new TaskCompletionSource<object>();
            if (cancellationToken == CancellationToken.None || !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    object result = MySqlHelper.ExecuteScalarImpl(connection, transaction, commandText, commandParameters);
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

        #region Actual command creation and execution private methods

        private static int ExecuteNonQueryImpl(MySqlConnection connection, MySqlTransaction transaction, string commandText, MySqlParameter[] commandParameters)
        {
            commandParameters = commandParameters ?? Array.Empty<MySqlParameter>();

            using (MySqlCommand mySqlCommand = new MySqlCommand())
            {
                mySqlCommand.Connection = connection;
                mySqlCommand.CommandText = commandText;
                mySqlCommand.CommandType = CommandType.Text;
                mySqlCommand.Transaction = transaction;

                foreach (MySqlParameter commandParameter in commandParameters)
                    mySqlCommand.Parameters.Add(commandParameter);
                
                int num = mySqlCommand.ExecuteNonQuery();
                mySqlCommand.Parameters.Clear();

                return num;
            }
        }

        private static DbDataReader ExecuteReaderImpl(MySqlConnection connection, MySqlTransaction transaction,
            string commandText, MySqlParameter[] commandParameters, bool externalConn)
        {
            commandParameters = commandParameters ?? Array.Empty<MySqlParameter>();

            using (MySqlCommand mySqlCommand = new MySqlCommand())
            {
                mySqlCommand.Connection = connection;
                mySqlCommand.CommandText = commandText;
                mySqlCommand.CommandType = CommandType.Text;
                mySqlCommand.Transaction = transaction;

                foreach (MySqlParameter commandParameter in commandParameters)
                    mySqlCommand.Parameters.Add(commandParameter);

                DbDataReader sqlDataReader = !externalConn
                    ? mySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                    : mySqlCommand.ExecuteReader();
                mySqlCommand.Parameters.Clear();
                return sqlDataReader;
            }
        }

        private static object ExecuteScalarImpl(MySqlConnection connection, MySqlTransaction transaction, string commandText, MySqlParameter[] commandParameters)
        {
            commandParameters = commandParameters ?? Array.Empty<MySqlParameter>();

            using (MySqlCommand mySqlCommand = new MySqlCommand())
            {
                mySqlCommand.Connection = connection;
                mySqlCommand.CommandText = commandText;
                mySqlCommand.CommandType = CommandType.Text;
                mySqlCommand.Transaction = transaction;

                foreach (MySqlParameter commandParameter in commandParameters)
                    mySqlCommand.Parameters.Add(commandParameter);

                object obj = mySqlCommand.ExecuteScalar();
                mySqlCommand.Parameters.Clear();
                return obj;
            }
        }

        #endregion

        private enum CharClass : byte
        {
            None,
            Quote,
            Backslash,
        }
    }
}