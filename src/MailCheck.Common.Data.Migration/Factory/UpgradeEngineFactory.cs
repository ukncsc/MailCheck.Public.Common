using System;
using System.Reflection;
using Amazon.SimpleSystemsManagement;
using DbUp.Engine;
using DbUp.Engine.Output;
using MailCheck.Common.Data.Abstractions;
using MailCheck.Common.Data.Implementations;
using MailCheck.Common.Data.Migration.Logging;
using MailCheck.Common.Data.Migration.Preprocessing;
using MailCheck.Common.Data.Migration.UpgradeEngine;
using MailCheck.Common.Data.Migration.Util;
using MailCheck.Common.Environment;
using MailCheck.Common.Logging;
using Microsoft.Extensions.DependencyInjection;
using MailCheck.Common.SSM;

namespace MailCheck.Common.Data.Migration.Factory
{
    public static class UpgradeEngineFactory
    {
        public enum DatabaseType
        {
            MySql,
            PostgreSql
        }

        public static IUpgradeEngine Create()
        {
            return Create(DatabaseType.MySql);
        }

        public static IUpgradeEngine Create(DatabaseType databaseType)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddTransient<IScriptAssemblyProvider>(_ => new ScriptAssemblyProvider(Assembly.GetEntryAssembly()))
                .AddEnvironment()
                .AddTransient<IAmazonSimpleSystemsManagement, CachingAmazonSimpleSystemsManagementClient>()
                .AddTransient<IUsernameProcessor, EnvironmentUsernameProcessor>()
                .AddTransient<IPasswordProcessor, ParameterStorePasswordProcessor>()
                .AddTransient<IPasswordGenerator, PasswordGenerator>()
                .AddTransient<IUpgradeLog, LogAdaptor>()
                .AddSerilogLogging();

            switch (databaseType)
            {
                case DatabaseType.PostgreSql:
                    serviceCollection.AddTransient<IUpgradeEngine, PostgresqlUpgradeEngineWrapper>();
                    serviceCollection.AddTransient<IScriptPreprocessor, PostgresScriptPreprocessor>();
                    serviceCollection.AddTransient<IConnectionInfoAsync, PostgresEnvironmentParameterStoreConnectionInfoAsync>();
                    break;
                case DatabaseType.MySql:
                    serviceCollection.AddTransient<IUpgradeEngine, MySqlUpgradeEngineWrapper>();
                    serviceCollection.AddTransient<IScriptPreprocessor, GrantPasswordSubstitutionPreprocessor>();
                    serviceCollection.AddTransient<IConnectionInfoAsync, MySqlEnvironmentParameterStoreConnectionInfoAsync>();
                    break;
                default:
                    throw new ArgumentException("Unexpected DatabaseType", databaseType.ToString());
            }

            return serviceCollection
                .BuildServiceProvider()
                .GetRequiredService<IUpgradeEngine>();
        }
    }
}
