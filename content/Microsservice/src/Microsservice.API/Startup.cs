using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsservice.API.Configurations;

namespace Microsservice.API
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private IConfiguration _configuration { get; }
        private readonly IWebHostEnvironment  _env;
        public Startup(IConfiguration configuration, IWebHostEnvironment  env)
        {
            _configuration = configuration;
            _env = env;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services.AddMvcCore();
          
            services.AddServices(mvcBuilder, _configuration, _env);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
            app.UseSwagger(_configuration);
        }
    }
}