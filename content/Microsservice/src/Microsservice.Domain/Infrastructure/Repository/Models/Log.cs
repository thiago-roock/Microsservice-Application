using System;
namespace Microsservice.Domain.Infrastructure.Repository.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Maquina { get; set; }
        public DateTimeOffset DataHora { get; set; }
        public string Tipo { get; set; }
        public string Mensagem { get; set; }                       
    }
}
