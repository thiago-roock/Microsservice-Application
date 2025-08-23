using System.Threading.Tasks;

namespace Microsservice.Domain.Infrastructure.ExternalServices
{
    public interface IRabbitMqService
    {
        Task<bool> PublicarMensagemAsync(string nomeFila, string mensagem);
    }
}
