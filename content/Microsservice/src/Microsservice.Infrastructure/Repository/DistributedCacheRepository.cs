using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsservice.Domain.Infrastructure.Repository.Models;
using Microsservice.Domain.Infrastructure.Repository;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsservice.Infrastructure.Repository
{
    public class DistributedCacheRepository : IDistributedCacheRepository
    {
        private readonly ILogger<DistributedCacheRepository> _logger;
        private readonly IDistributedCache _cache;
        private readonly IConfiguration _configuration;
        private TimeSpan timeExpiredCached;

        public DistributedCacheRepository(IDistributedCache cache, IConfiguration configuration, ILogger<DistributedCacheRepository> logger)
        {
            _cache = cache;
            _configuration = configuration;
            _logger = logger;

            _logger.LogInformation("Carregando configurações do redis...");

            timeExpiredCached = TimeSpan.FromSeconds(int.Parse(_configuration["CACHE_TIME_EXPIRED_CACHED"]));

            _logger.LogInformation("Configurações carregadas com sucesso:");
            _logger.LogInformation($"Nome da instância: {_configuration["CACHE_INSTANCE_NAME"]}");
            _logger.LogInformation($"Servidor: {_configuration["CACHE_CONFIGURATION_URL"]}");
            _logger.LogInformation($"Tempo de expiração: {timeExpiredCached}");
        }

        public async Task<MicrosserviceExternalServiceModel> GetItemCache(string keyIdentify)
        {
            MicrosserviceExternalServiceModel FromCache = null;
            try
            {
                var ObjCache = await _cache.GetStringAsync($"{keyIdentify}");
                if (ObjCache is not null)
                    FromCache = JsonSerializer.Deserialize<MicrosserviceExternalServiceModel>(ObjCache);

                _logger.LogInformation($"Get {keyIdentify}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " +
                            $"Mensagem: {ex.Message}");

                throw new ArgumentException($"Exceção: {ex.GetType().FullName} | " +
                               $"Mensagem: {ex.Message}");
            }
            return FromCache;
        }

        public async Task InsertItemCache(string keyIdentify, MicrosserviceExternalServiceModel cache)
        {
            try
            {
                var value = JsonSerializer.Serialize(cache);
                _logger.LogInformation($"inserindo {keyIdentify}: {value}");
                await _cache.SetStringAsync($"{keyIdentify}", value);
                _logger.LogInformation("inserção realizada com sucesso no Redis!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " +
                            $"Mensagem: {ex.Message}");

                throw new ArgumentException($"Exceção: {ex.GetType().FullName} | " +
                               $"Mensagem: {ex.Message}");
            }
        }
    }
}
