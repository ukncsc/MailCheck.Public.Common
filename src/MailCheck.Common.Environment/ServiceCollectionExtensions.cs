using MailCheck.Common.Environment.Abstractions;
using MailCheck.Common.Environment.Implementations;
using Microsoft.Extensions.DependencyInjection;
using MailCheck.Common.Util;

namespace MailCheck.Common.Environment
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEnvironment(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .TryAddSingleton<IEnvironment, EnvironmentWrapper>()
                .TryAddSingleton<IEnvironmentVariables, EnvironmentVariables>();

            return serviceCollection;
        }
    }
}
