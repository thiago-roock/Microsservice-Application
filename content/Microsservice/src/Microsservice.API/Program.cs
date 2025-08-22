using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsservice.API;

namespace Microservice.API
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main()
        {
            CreateWebHostBuilder(new string[] { }).Build().Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            //if (!hostingContext.HostingEnvironment.IsDevelopment())
                            //    config.AddConsulDefault("config/project-name/appsettings.json", 60);

                            config.AddEnvironmentVariables();
                        })
                        .ConfigureKestrel((context, options) =>
                        {
                            if (context.HostingEnvironment.IsDevelopment())
                            {
                                // ?? Em DEV não força nada ? usa launchSettings.json
                            }
                            else
                            {
                                // ?? Em PRODUÇÃO ? seta HTTP/1 na porta 80 e HTTP/2 na porta 5005
                                options.Listen(IPAddress.Any, 80, listenOptions =>
                                {
                                    listenOptions.Protocols = HttpProtocols.Http1;
                                });
                                options.Listen(IPAddress.Any, 5005, listenOptions =>
                                {
                                    listenOptions.Protocols = HttpProtocols.Http2;
                                });
                            }

                        })
                        .UseStartup<Startup>();
        }
    }
}