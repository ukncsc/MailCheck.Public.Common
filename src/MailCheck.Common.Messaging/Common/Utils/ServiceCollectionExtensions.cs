using System;
using System.Linq;
using System.Reflection;
using MailCheck.Common.Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MailCheck.Common.Messaging.Common.Utils
{
    internal static class ServiceCollectionExtensions
    {
        private static readonly Type OpenHandleInterface = typeof(IHandle<>);
        private static readonly Func<Type, bool> IsClosedHandleInterface = type =>
            type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == OpenHandleInterface;

        /// <summary>
        /// Gets IHandle<T> interface registrations for handlers just registered as a
        /// concrete type e.g. collection.AddTransient<MyHandler>();
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        internal static IServiceCollection ExpandConcreteHandlers(this IServiceCollection collection)
        {
            collection
                .Where(descriptor => descriptor.ServiceType.IsClass)
                .SelectMany(descriptor =>
                    descriptor.ServiceType.GetTypeInfo().ImplementedInterfaces
                        .Where(IsClosedHandleInterface)
                        .Select(handleInterface => new ServiceDescriptor(handleInterface, descriptor.ServiceType, descriptor.Lifetime))
                )
                .ToList()
                .ForEach(collection.Add);

            var duplicateRegistrations = collection
                .Select(descriptor => descriptor.ServiceType)
                .Where(IsClosedHandleInterface)
                .GroupBy(handleInterface => handleInterface)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key.FullName)
                .ToArray();

            if (duplicateRegistrations.Length > 0)
            {
                throw new System.Exception($"Duplicate handler registrations found for:{System.Environment.NewLine}\t{string.Join(System.Environment.NewLine + "\t", duplicateRegistrations)}");
            }

            return collection;
        }
    }
}
