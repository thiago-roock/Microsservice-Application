using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Microsservice.API.Controllers;
using Microsservice.Domain.Commands;
using Xunit;
using System.Threading;
using System.Threading.Tasks;

namespace Microsservice.Tests.Unit.Controller
{
    public class MicrosserviceControllerTests
    {
        [Fact]
        public async Task Should_Send_Get_Microsservice_Command_To_Mediator()
        {
            var mediatorMock = new Mock<IMediator>();

            var logger = new Mock<ILogger<MicrosserviceController>>();

            var retorno = new MediatR.Unit();

            mediatorMock.Setup(x => x.Send(It.IsAny<MicrosserviceCommand>(), CancellationToken.None)).ReturnsAsync(retorno);
            
            var controller = new MicrosserviceController(mediatorMock.Object);
            
            var result = await controller.Get(new MicrosserviceCommand());
            
            mediatorMock.VerifyAll();
            
            Assert.NotNull(result);
        }
    }
}