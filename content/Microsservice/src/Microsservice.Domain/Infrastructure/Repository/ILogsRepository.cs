using Microsservice.Domain.Infrastructure.Repository.Models;
using System;
using System.Collections.Generic;

namespace Microsservice.Domain.Infrastructure.Repository
{
    public interface ILogsRepository
    {
        #region buscas
        IEnumerable<Log> ObterLogsPorPeriodo(DateTimeOffset dataHoraInicio, DateTimeOffset dataHoraFim);
        #endregion buscas

        #region inserir
        bool SalvarLog(Log log);
        #endregion inserir

        #region atualizar
        bool AtualizarLog(Log log);
        #endregion atualizar

        #region deletar
        bool DeletarLog(int idLog);
        #endregion deletar
    }
}
