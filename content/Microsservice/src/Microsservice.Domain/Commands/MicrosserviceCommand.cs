using MediatR;

namespace Microsservice.Domain.Commands
{
    public class MicrosserviceCommand: IRequest<Unit>
    {
        public string Name { get; set; }

    }
}