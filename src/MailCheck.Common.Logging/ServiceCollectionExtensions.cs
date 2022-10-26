using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MailCheck.Common.Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSerilogLogging(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(new RenderedJsonFormatter())
                .CreateLogger();

            return services
                .AddLogging(_ => _.AddSerilog());
        }
    }
}
