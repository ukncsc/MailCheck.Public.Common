using System.Threading.Tasks;
using DbUp;
using DbUp.Engine;
using DbUp.Engine.Output;
using MailCheck.Common.Data.Abstractions;
using MailCheck.Common.Data.Migration.Util;
using MailCheck.Common.Data.Util;

namespace MailCheck.Common.Data.Migration.UpgradeEngine
{
    public class PostgresqlUpgradeEngineWrapper : IUpgradeEngine
    {
        private readonly IConnectionInfoAsync _connectionInfoAsync;
        private readonly IScriptPreprocessor _preprocessor;
        private readonly IScriptAssemblyProvider _scriptAssemblyProvider;
        private readonly IUpgradeLog _log;

        public PostgresqlUpgradeEngineWrapper(IConnectionInfoAsync connectionInfoAsync,
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

            string database = PostgresConnectionStringHelpers.GetDatabaseName(connectionString);

            DbUp.Engine.UpgradeEngine upgradeEngine = CreateUpgradeEngine(connectionString);

            EnsureDatabase.For.PostgresqlDatabase(connectionString, _log);

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

        private DbUp.Engine.UpgradeEngine CreateUpgradeEngine(string connectionString)
        {
            return DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .JournalToPostgresqlTable("public", "schema_version")
                .WithScriptsEmbeddedInAssembly(_scriptAssemblyProvider.ScriptAssembly)
                .WithPreprocessor(_preprocessor)
                .LogTo(_log)
                .Build();
        }
    }
}