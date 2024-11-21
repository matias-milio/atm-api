using ATM.UseCases.CardHolder.Login;
using ATM.Api.RequestModels;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;
using ATM.Api.RequestModels.Validators;

namespace ATM.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthenticationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        [Route("login")]
        [SwaggerOperation(
            Summary = "Logs in into the system and gets the access token.",
            Description = "This endpoint is used to get the authorization token for using the other endpoints, using the " +
            "Card Number and PIN to authenticate, if the PIN is inputted 3 times wrong the card will be rendered inactive. ",
            OperationId = "login"
        )]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {

            var loginValidator = new LoginRequestModelValidator();
            var logingValidatorResult = await loginValidator.ValidateAsync(request, cancellationToken);

            if (!logingValidatorResult.IsValid)
                return BadRequest(logingValidatorResult.Errors.Select(e => e.ErrorMessage));

            var query = new LoginCommand(request.CardNumber, request.PIN);            
            var loginResult = (await _mediator.Send(query, cancellationToken))
                             .Match(resultValue => resultValue, error => error);

            if (loginResult.Error != null)
                return Problem(loginResult.Error.Message, null, loginResult.Error.HttpStatusCode);
            else
                return Ok(loginResult.Value);
        }        
    }
}
