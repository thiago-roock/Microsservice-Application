using System.Threading.Tasks;
using MediatR;
using Microsservice.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
namespace Microsservice.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MicrosserviceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public MicrosserviceController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("/Microsservice/{name}")]
        public async Task<IActionResult> Get([FromRoute]MicrosserviceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}