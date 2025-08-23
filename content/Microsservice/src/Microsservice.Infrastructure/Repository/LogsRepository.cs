using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sample.Domain.Infrastructure.Repository;
using Sample.Domain.Infrastructure.Repository.Models;
using System;
using System.Collections.Generic;

namespace Microsservice.Infrastructure.Repository
{
    public class LogsRepository : ILogsRepository
    {
        private readonly DbConexao _conexao;
        private readonly ILogger<LogsRepository> _logger;
        private readonly IConfiguration _configuration;

        public LogsRepository(DbConexao conexao, IConfiguration configuration, ILogger<LogsRepository> logger)
        {

            _logger = logger;
            _logger.LogInformation("Carregando configurações do banco de dados...");
            _configuration = configuration;
            _conexao = conexao;
            _logger.LogInformation("Configurações carregadas com sucesso:");
            _logger.LogInformation($"ConnectionStrings: {_configuration["DefaultConnection"]}");
        }

        public IEnumerable<Log> ObterLogsPorPeriodo(DateTimeOffset dataHoraInicio, DateTimeOffset dataHoraFim)
        {
            try
            {
                using var con = _conexao.Connection;


                IEnumerable<Log> lista = con.Query<Log>(QueryObterLogsPorPeriodo(), new { dataHoraInicio, dataHoraFim }, _conexao.Transaction);

                #region QueryObterLogsPorPeriodo

                static string QueryObterLogsPorPeriodo() => @"SELECT * FROM Logs WITH (NOLOCK) 
                                                              WHERE DataHora BETWEEN @dataHoraInicio AND @dataHoraFim";

                #endregion QueryObterLogsPorPeriodo

                return lista;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " +
                                           $"Mensagem: {ex.Message}");

                throw new Exception($"Exceção: {ex.GetType().FullName} | " +
                               $"Mensagem: {ex.Message}");
            }
        }

        public bool SalvarLog(Log log)
        {
            try
            {
                using var con = _conexao.Connection;
                int resultado = con.ExecuteScalar<int>(QuerySalvarLog(), new {  Usuario = log.Usuario,
                                                                                Maquina = log.Maquina,
                                                                                DataHora = log.DataHora,
                                                                                Tipo = log.Tipo,
                                                                                Mensagem = log.Mensagem
                                                                             }, _conexao.Transaction);

                #region QuerySalvarLog
                static string QuerySalvarLog() => @"INSERT INTO Logs
                                                                    (
                                                                        Usuario,
                                                                        Maquina,
                                                                        DataHora,
                                                                        Tipo,
                                                                        Mensagem
                                                                    )
                                                                    VALUES
                                                                    (
	                                                                    @Usuario,
	                                                                    @Maquina,
	                                                                    @DataHora,
	                                                                    @Tipo,
	                                                                    @Mensagem
                                                                    )

                                                                    SELECT
	                                                                    Id
                                                                    FROM 
	                                                                    Logs
                                                                    WHERE
	                                                                    Id=SCOPE_IDENTITY()";
                #endregion QuerySalvarLog
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " +
                                           $"Mensagem: {ex.Message}");

                throw new Exception($"Exceção: {ex.GetType().FullName} | " +
                               $"Mensagem: {ex.Message}");
            }
        }

        public bool AtualizarLog(Log log)
        {
            try
            {
                using var con = _conexao.Connection;
                int resultado = con.ExecuteScalar<int>(AtualizarLog(), new
                {
                    Usuario = log.Usuario,
                    Maquina = log.Maquina,
                    DataHora = log.DataHora,
                    Tipo = log.Tipo,
                    Mensagem = log.Mensagem
                }, _conexao.Transaction);

                #region QueryAtualizarLog
                static string AtualizarLog() => @"UPDATE Logs
                                                                SET Usuario = @Usuario,
                                                                    Maquina = @Maquina,
                                                                    DataHora = @DataHora,
                                                                    Tipo = @Tipo,
                                                                    Mensagem = @Mensagem
                                                                WHERE IdLog = @idLog";
                #endregion QueryAtualizarLog

                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " +
                                           $"Mensagem: {ex.Message}");

                throw new Exception($"Exceção: {ex.GetType().FullName} | " +
                               $"Mensagem: {ex.Message}");
            }
        }

        public bool DeletarLog(int idLog)
        {
            try
            {
                using var con = _conexao.Connection;

                int resultado = con.Execute(QueryDeletarLog(), new
                {
                    IdLog = idLog
                }, _conexao.Transaction);
                #region QueryDeletarLog

                static string QueryDeletarLog() => @"DELETE FROM Logs WHERE IdLog = @idLog";
                #endregion QueryDeletarLog

                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exceção: {ex.GetType().FullName} | " +
                                           $"Mensagem: {ex.Message}");

                throw new Exception($"Exceção: {ex.GetType().FullName} | " +
                               $"Mensagem: {ex.Message}");
            }
        }
    }
}

