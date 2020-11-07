using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsservice.Domain.Commands;
using Microsservice.Domain.Infrastructure.ExternalServices;
namespace Microsservice.Domain.Handlers
{
    public class MicrosserviceHandler : IRequestHandler<MicrosserviceCommand, Unit>
    {
        private readonly IMicrosserviceExternalService microsserviceExternalService;
        public MicrosserviceHandler(IMicrosserviceExternalService microsserviceExternalService)
        {
            this.microsserviceExternalService = microsserviceExternalService;
        }
        public async Task<Unit> Handle(MicrosserviceCommand request, CancellationToken cancellationToken)
        {
            var retorno = await microsserviceExternalService.Post(request);
            return new Unit();
        }
    }
}