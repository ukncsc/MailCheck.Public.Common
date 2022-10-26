using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MailCheck.Common.Util
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection TryAddTransient<TService, TImplementation>(
            this IServiceCollection serviceContainer)
            where TService : class
            where TImplementation : class, TService
        {
            ServiceCollectionDescriptorExtensions.TryAddTransient<TService, TImplementation>(serviceContainer);

            return serviceContainer;
        }

        public static IServiceCollection TryAddScoped<TService, TImplementation>(
            this IServiceCollection serviceContainer)
            where TService : class
            where TImplementation : class, TService
        {
            ServiceCollectionDescriptorExtensions.TryAddScoped<TService, TImplementation>(serviceContainer);

            return serviceContainer;
        }

        public static IServiceCollection TryAddSingleton<TService, TImplementation>(
            this IServiceCollection serviceContainer)
            where TService : class
            where TImplementation : class, TService
        {
            ServiceCollectionDescriptorExtensions.TryAddSingleton<TService, TImplementation>(serviceContainer);

            return serviceContainer;
        }
    }
}
