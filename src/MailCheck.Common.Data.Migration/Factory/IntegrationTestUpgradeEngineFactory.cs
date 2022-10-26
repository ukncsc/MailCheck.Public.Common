using System.Reflection;
using DbUp.Engine;
using DbUp.Engine.Output;
using MailCheck.Common.Data.Abstractions;
using MailCheck.Common.Data.Implementations;
using MailCheck.Common.Data.Migration.Logging;
using MailCheck.Common.Data.Migration.Preprocessing;
using MailCheck.Common.Data.Migration.UpgradeEngine;
using MailCheck.Common.Data.Migration.Util;
using MailCheck.Common.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MailCheck.Common.Data.Migration.Factory
{
    public static class IntegrationTestUpgradeEngineFactory
    {
        public static IUpgradeEngine Create(string connectionString, Assembly assembly)
        {
            return AddLogging(new ServiceCollection()
                    .AddTransient<IScriptAssemblyProvider>(_ => new ScriptAssemblyProvider(assembly))
                    .AddTransient<IConnectionInfoAsync>(_ => new StringConnectionInfoAsync(connectionString))
                    .AddTransient<IUpgradeEngine, MySqlUpgradeEngineWrapper>()
                    .AddTransient<IScriptPreprocessor, GrantPasswordSubstitutionPreprocessor>()
                    .AddTransient<IUsernameProcessor, DummyUsernameProcessor>()
                    .AddTransient<IPasswordProcessor, DummyPasswordProcessor>()
                    .AddTransient<IPasswordGenerator, PasswordGenerator>()
                    .AddTransient<IUpgradeLog, LogAdaptor>())
                .BuildServiceProvider()
                .GetRequiredService<IUpgradeEngine>();
        }

        private static IServiceCollection AddLogging(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(new RenderedJsonFormatter())
                .CreateLogger();

            return services
                .AddLogging(_ => _.AddSerilog());
        }
    }
}