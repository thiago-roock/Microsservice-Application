using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RabbitMQ.Client;
using Microsservice.Domain.Commands;
using Microsservice.Domain.Infrastructure.ExternalServices;
using Microsservice.Infrastructure;
using Microsservice.Infrastructure.ExternalServices;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Microsservice.API.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class Configurations
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IMvcCoreBuilder mvcBuilder, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddScoped<IMicrosserviceExternalService, MicrosserviceExternalService>();
            services.AddHttpClient("Microsservice", c =>
            {
                c.BaseAddress = new Uri(configuration["Microsservice_ENDPOINT"]);
            });
           
            services.AddInfrastructureServices(configuration,mvcBuilder);
            return services;
        }
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration,IMvcCoreBuilder mvcBuilder)
        {
            var domainAssembly = typeof(MicrosserviceCommand).Assembly;
            
            mvcBuilder
                .AddApiExplorer();
 
                
            services
                .AddValidatorsFromAssembly(domainAssembly)
                .AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(domainAssembly);
                })
                .AddSwagger(configuration)
                .AddLogging()
                .AddCache(configuration)
                .AddHealthChecks();

            services.AddScoped<DbConexao>();

            // Configura fábrica do RabbitMQ (vem do appsettings.json)
            services.AddSingleton(sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = configuration["RabbitMq:HostName"] ?? "localhost",
                    UserName = configuration["RabbitMq:UserName"] ?? "guest",
                    Password = configuration["RabbitMq:Password"] ?? "guest",
                    Port = int.Parse(configuration["RabbitMq:Port"] ?? "5672"),
                };
                return factory;
            });


            // Registra serviço
            services.AddSingleton<IRabbitMqService, RabbitMqService>();

            // OpenTelemetry + OTLP (Jaeger v2)
            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService("Microsservice")) // Define o nome do serviço
                .WithTracing(builder =>
                {
                    builder
                        // Instrumentação automática para ASP.NET Core
                        .AddAspNetCoreInstrumentation()
                        // Instrumentação automática para HttpClient
                        .AddHttpClientInstrumentation()
                        // Exporta spans para o console (opcional, útil para debug)
                        .AddConsoleExporter()
                        // Spans manuais via ActivitySource("Sample")
                        .AddSource("Microsservice")
                       // Exporta spans para Jaeger via OTLP/gRPC
                       .AddOtlpExporter(opt =>
                       {
                           var jaegerHost = configuration["Jaeger:Host"] ?? "localhost";
                           opt.Endpoint = new Uri($"http://{jaegerHost}:4317"); // gRPC
                       });

                });

            return services;
        }
        private static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration["CACHE_INSTANCE_NAME"]))
                throw new ArgumentException("The parameter CACHE_INSTANCE_NAME is null or empty.");
            if (string.IsNullOrEmpty(configuration["CACHE_CONFIGURATION_URL"]))
                throw new ArgumentException("The parameter CACHE_CONFIGURATION_URL is null or empty.");
            if (string.IsNullOrEmpty(configuration["CACHE_TIME_EXPIRED_CACHED"]))
                throw new ArgumentException("The parameter CACHE_TIME_EXPIRED_CACHED is null or empty.");
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["CACHE_CONFIGURATION_URL"];
                options.InstanceName = configuration["CACHE_INSTANCE_NAME"];
                options.ConfigurationOptions = new ConfigurationOptions()
                {
                    AbortOnConnectFail = false,
                    EndPoints =
                    {
                        {
                            configuration["CACHE_CONFIGURATION_URL"],
                            int.Parse(configuration["CACHE_CONFIGURATION_PORT"])
                        }
                    },
                };
            });

            return services;
        }
        private static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var assemblyName = Assembly.GetCallingAssembly().GetName();
            var version = string.Concat("v", assemblyName.Version);
            services.AddSwaggerGen(setup =>
            {
                setup.CustomSchemaIds(x => x.FullName);
                setup.EnableAnnotations();
                setup.DescribeAllParametersInCamelCase();
                var swaggerInfo = new OpenApiInfo
                {
                    Title = assemblyName.Name,
                    Version = version,
                    Description = "Microsservice",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "teste",
                        Email = "teste@teste.com",
                        Url = new Uri("https://example.com/teste"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "licenses Microsservice",
                        Url = new Uri("https://example.com/license"),
                    }
                };
                setup.SwaggerDoc(swaggerInfo.Version, swaggerInfo);
                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                var xmlPath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath,
                string.Concat(assemblyName.Name, ".xml"));
                // Set the comments path for the Swagger JSON and UI.
                if (File.Exists(xmlPath))
                    setup.IncludeXmlComments(xmlPath);
            });
            services.AddSwaggerGenNewtonsoftSupport();
            return services;
        }
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
        {
            var assemblyName = Assembly.GetCallingAssembly().GetName();
            var version = string.Concat("v", assemblyName.Version);
            var projectName = string.Empty;

            //Só adiciona o projectName em Produção
            if (env.IsProduction())
                projectName = configuration["PROJECT_NAME"] ?? string.Empty;

            app.UseSwagger(setup =>
            {
                setup.PreSerializeFilters.Add(
                    (swaggerDoc, httpReq)
                    => swaggerDoc.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer
                            {
                                Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/{projectName}"
                            }
                        });
            });
            app.UseSwaggerUI(setup =>
            {
                setup.RoutePrefix = string.Empty;
                setup.SwaggerEndpoint($"./swagger/{version}/swagger.json", version);
            });
            return app;
        }
    }
}