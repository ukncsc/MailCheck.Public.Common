using System.Threading.Tasks;
using DbUp;
using DbUp.Engine;
using DbUp.Engine.Output;
using MailCheck.Common.Data.Abstractions;
using MailCheck.Common.Data.Migration.Util;
using MailCheck.Common.Data.Util;

namespace MailCheck.Common.Data.Migration.UpgradeEngine
{
    public class MySqlUpgradeEngineWrapper : IUpgradeEngine
    {
        private readonly IConnectionInfoAsync _connectionInfoAsync;
        private readonly IScriptPreprocessor _preprocessor;
        private readonly IScriptAssemblyProvider _scriptAssemblyProvider;
        private readonly IUpgradeLog _log;

        public MySqlUpgradeEngineWrapper(IConnectionInfoAsync connectionInfoAsync,
            IScriptPreprocessor preprocessor,
            IScriptAssemblyProvider scriptAssemblyProvider,
            IUpgradeLog log)
        {
            _connectionInfoAsync = connectionInfoAsync;
            _preprocessor = preprocessor;
            _scriptAssemblyProvider = scriptAssemblyProvider;
            _log = log;
        }

        public async Task<int> PerformUpgrade()
        {
            string connectionString = await _connectionInfoAsync.GetConnectionStringAsync();

            string database = MySqlConnectionStringHelpers.GetDatabaseName(connectionString);

            DbUp.Engine.UpgradeEngine upgradeEngine = CreateUpgradeEngine(connectionString, database);

            EnsureDatabase.For.MySqlDatabase(connectionString, _log);

            DatabaseUpgradeResult databaseUpgradeResult = upgradeEngine.PerformUpgrade();

            if (databaseUpgradeResult.Successful)
            {
                _log.WriteInformation("Successfully migrated database {Database}.", database);
                return 0;
            }

            _log.WriteError("Failed to migrate database {Database} with error: {ErrorMessage} {StackTrace}",
                database, databaseUpgradeResult.Error.Message, databaseUpgradeResult.Error.StackTrace);
            return -1;
        }

        private DbUp.Engine.UpgradeEngine CreateUpgradeEngine(string connectionString, string database)
        {
            return DeployChanges.To
                .MySqlDatabase(connectionString)
                .JournalToMySqlTable(database, "schema_version")
                .WithScriptsEmbeddedInAssembly(_scriptAssemblyProvider.ScriptAssembly)
                .WithPreprocessor(_preprocessor)
                .LogTo(_log)
                .Build();
        }
    }
}