using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
namespace Microsservice.API
{
    [ExcludeFromCodeCoverage]
    public static class Configurations
    {
        public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            return services;
        }
    }
}