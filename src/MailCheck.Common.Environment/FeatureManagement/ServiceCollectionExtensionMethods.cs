using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace MailCheck.Common.Environment.FeatureManagement
{
    public static class ServiceCollectionExtensionMethods
    {
        /// <summary>
        /// Executes an action conditionally based on the presence of the supplied feature toggle in the environment variable ACTIVE_FEATURES
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="featureName">The key in ACTIVE_FEATURES that enables the feature</param>
        /// <param name="featureActiveRegistrations">The action to perform if <paramref name="featureName"/> is enabled</param>
        /// <param name="featureInactiveRegistrations">The action to perform if <paramref name="featureName"/> is not enabled</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddConditionally(
            this IServiceCollection services,
            string featureName,
            Action<IServiceCollection> featureActiveRegistrations,
            Action<IServiceCollection> featureInactiveRegistrations
        )
        {
            string activeFeaturesValue = System.Environment.GetEnvironmentVariable("ACTIVE_FEATURES") ?? string.Empty;

            bool featureActive = activeFeaturesValue.Split(',').Select(x => x.Trim()).Contains(featureName);

            if (featureActive)
            {
                featureActiveRegistrations(services);
            }
            else
            {
                featureInactiveRegistrations(services);
            }

            return services;
        }
    }
}