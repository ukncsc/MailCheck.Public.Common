using System;
using MySql.Data.MySqlClient;

namespace MailCheck.Common.Data.Util
{
    public static class MySqlConnectionStringHelpers
    {
        public static string GetUserId(string connectionString, bool throwIfNotExists = true)
        {
            return GetConnectionStringValue(connectionString, "UserID", _ => _.UserID, throwIfNotExists);
        }

        public static string GetPassword(string connectionString, bool throwIfNotExists = true)
        {
            return GetConnectionStringValue(connectionString, "Password", _ => _.Password, throwIfNotExists);
        }

        public static string GetDatabaseName(string connectionString, bool throwIfNotExists = true)
        {
            return GetConnectionStringValue(connectionString, "Database", _ => _.Database, throwIfNotExists);
        }

        private static string GetConnectionStringValue(string connectionString, string valueName, Func<MySqlConnectionStringBuilder, string> valueGetter, bool throwIfNotExists = true)
        {
            try
            {
                MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

                string value = valueGetter(connectionStringBuilder);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }
            catch
            {
                //swallow and exceptions here
            }

            if (throwIfNotExists)
            {
                throw new InvalidOperationException($"The connection string does not specify a {valueName}.");
            }

            return null;
        }
    }
}
