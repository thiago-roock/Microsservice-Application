using System.Threading.Tasks;
using MediatR;
using Microsservice.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        /// <summary>
        /// Digite aqui a funcionalidade project-name
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpGet("/Microsservice/{name}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Get([FromRoute]MicrosserviceCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}