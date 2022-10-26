using System;
using System.Data;
using DbUp;
using DbUp.Builder;
using DbUp.Engine.Output;
using DbUp.MySql;
using MailCheck.Common.Data.Migration.UpgradeEngine;
using MailCheck.Common.Data.Util;
using MySql.Data.MySqlClient;

namespace MailCheck.Common.Data.Migration.Util
{
    public static class MySqlExtensions
    {
        public static UpgradeEngineBuilder JournalToMySqlTable(this UpgradeEngineBuilder builder, string schema, string table)
        {
            builder.Configure(c => c.Journal = new RunEverytimeJournal(new MySqlTableJournal(() => c.ConnectionManager, () => c.Log, schema, table)));
            return builder;
        }

        //https://gist.github.com/chriswithpants/678ef5c9f9fe23d3b21fdc0ec80fe1f4
        public static void MySqlDatabase(this SupportedDatabasesForEnsureDatabase supported, string connectionString, IUpgradeLog logger)
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

            MySqlConnectionStringBuilder connectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);

            string initialCatalog = MySqlConnectionStringHelpers.GetDatabaseName(connectionString);

            connectionStringBuilder.Database = null;
            
            MySqlConnectionStringBuilder maskedConnectionStringBuilder = new MySqlConnectionStringBuilder(connectionStringBuilder.ConnectionString)
            {
                Password = string.Empty.PadRight(connectionStringBuilder.Password.Length, '*')
            };

            logger.WriteInformation("Using connection string {0}", maskedConnectionStringBuilder.ConnectionString);

            using (MySqlConnection connection = new MySqlConnection(connectionStringBuilder.ConnectionString))
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
                using (MySqlCommand mySqlCommand = new MySqlCommand($"CREATE DATABASE IF NOT EXISTS {initialCatalog}", connection) { CommandType = CommandType.Text })
                {
                    mySqlCommand.ExecuteNonQuery();
                }

                logger.WriteInformation("Ensured database {0} exists", initialCatalog);
            }
        }
    }
}
