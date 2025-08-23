using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sample.Domain.Commands;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

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
            // Propaga CodigoTracing se existir na Activity corrente
            var codigoTracing = Activity.Current?.GetBaggageItem("CodigoTracing");
            if (string.IsNullOrEmpty(codigoTracing))
                // Cria código de tracing único
                codigoTracing = Guid.NewGuid().ToString();

            // Adiciona como tag e baggage para propagação
            Activity.Current?.SetTag("CodigoTracing", codigoTracing);
            Activity.Current?.AddBaggage("CodigoTracing", codigoTracing);

            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}