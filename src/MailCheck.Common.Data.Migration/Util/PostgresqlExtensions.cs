using System;
using System.Data;
using DbUp;
using DbUp.Builder;
using DbUp.Engine.Output;
using DbUp.Postgresql;
using MailCheck.Common.Data.Util;
using Npgsql;

namespace MailCheck.Common.Data.Migration.Util
{
    public static class PostgresqlExtensions
    {
        public static UpgradeEngineBuilder JournalToPostgresqlTable(this UpgradeEngineBuilder builder, string schema, string table)
        {
            builder.Configure(c => c.Journal = new PostgresqlTableJournal(() => c.ConnectionManager, () => c.Log, schema, table));
            return builder;
        }
        
        public static void PostgresqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, IUpgradeLog logger)
        {
            if (supported == null)
            {
                throw new ArgumentNullException(nameof(supported));
            }

            if (string.IsNullOrEmpty(connectionString) || connectionString.Trim() == string.Empty)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            NpgsqlConnectionStringBuilder connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);

            string initialCatalog = PostgresConnectionStringHelpers.GetDatabaseName(connectionString);

            connectionStringBuilder.Database = "postgres"; // default database

            NpgsqlConnectionStringBuilder maskedConnectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionStringBuilder.ConnectionString)
            {
                Password = string.Empty.PadRight(connectionStringBuilder.Password.Length, '*')
            };

            logger.WriteInformation("Using connection string {0}", maskedConnectionStringBuilder.ConnectionString);

            using (NpgsqlConnection connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    logger.WriteError("Unable to open database connection to {0}: {1}",
                        connection.ConnectionString, connection.Database, ex);
                    throw;
                }

                try
                {
                    using (NpgsqlCommand npgSqlCommand = new NpgsqlCommand($"CREATE DATABASE {initialCatalog}", connection) { CommandType = CommandType.Text })
                    {
                        npgSqlCommand.ExecuteNonQuery();
                    }
                }
                catch (PostgresException ex)
                {
                    if (ex.SqlState != "42P04") //duplicate_database
                    {
                        throw;
                    }
                }

                logger.WriteInformation("Ensured database {0} exists", initialCatalog);
            }
        }
    }
}
