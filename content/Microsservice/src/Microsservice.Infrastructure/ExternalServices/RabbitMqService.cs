using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Microsservice.Domain.Infrastructure.ExternalServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsservice.Infrastructure.ExternalServices
{
    public class RabbitMqService : IRabbitMqService, IAsyncDisposable
    {
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        private readonly ILogger<RabbitMqService> _logger;
        private readonly IConfiguration _configuration;
        private static readonly ActivitySource ActivitySource = new("Microsservice.RabbitMq");


        public RabbitMqService(ConnectionFactory factory, ILogger<RabbitMqService> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _logger.LogInformation("Carregando configurações do RabbitMQ...");
            // Cria conexão e canal (aqui ainda síncrono no construtor, mas vindo da DI)
            _connection = factory.CreateConnectionAsync("SampleRabbitMqService").GetAwaiter().GetResult();
            _logger.LogInformation("Conexão com RabbitMQ estabelecida com sucesso.");
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
            _logger.LogInformation("Canal com RabbitMQ criado com sucesso.");
            // Declara fila
            _channel.QueueDeclareAsync(
                queue: _configuration["RBMQ_NOME_FILA"],
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: CancellationToken.None
            ).GetAwaiter().GetResult();
            _logger.LogInformation("Fila declarada com sucesso.");
            _logger.LogInformation("Configurações carregadas com sucesso:");
        }

        public async Task<bool> PublicarMensagemAsync(string nomeFila, string mensagem)
        {
            using var activity = ActivitySource.StartActivity("RabbitMQ PublicarMensagem", ActivityKind.Producer);

            try
            {
                var body = Encoding.UTF8.GetBytes(mensagem);
                var props = new BasicProperties();
                props.ContentType = "application/json";
                props.Headers = new Dictionary<string, object>();

                // 🔹 Propagar o trace context (traceparent, tracestate)
                if (activity != null)
                {
                    var context = activity.Context;

                    props.Headers["traceparent"] = $"00-{context.TraceId}-{context.SpanId}-01";

                    if (!string.IsNullOrEmpty(context.TraceState))
                        props.Headers["tracestate"] = context.TraceState;

                    // Propaga CodigoTracing
                    var codigoTracing = Activity.Current?.GetBaggageItem("CodigoTracing") ?? Guid.NewGuid().ToString();
                    props.Headers["CodigoTracing"] = codigoTracing;

                    activity?.SetTag("CodigoTracing", codigoTracing);
                }

                // Publica mensagem de forma assíncrona
                await _channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: _configuration["RBMQ_NOME_FILA"],
                    mandatory: false,
                    basicProperties: props,
                    body: body,
                    cancellationToken: default);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " +
                           $"Mensagem: {ex.Message}");
                return false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel != null) await _channel.CloseAsync();
            if (_connection != null) await _connection.CloseAsync();
        }
    }
}

