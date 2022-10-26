using Microsoft.Extensions.DependencyInjection;

namespace MailCheck.Common.Messaging.Abstractions
{
    public interface IStartUp
    {
        void ConfigureServices(IServiceCollection services);
    }
}