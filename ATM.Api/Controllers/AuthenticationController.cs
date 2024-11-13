using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Azure.Core;
using ATM.UseCases.CardHolder.Login;
using System.Threading;

namespace ATM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("login")]
        public async Task<IActionResult> Login([FromQuery] string cardNumber, [FromQuery] string PIN, CancellationToken cancellationToken)
        {
            var query = new LoginCommand(cardNumber, PIN);            
            var result = await _mediator.Send(query, cancellationToken);
            result.Match(resultValue => resultValue,error => error);
            return Ok(result);
        }
    }
}
