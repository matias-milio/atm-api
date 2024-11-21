using ATM.Api.RequestModels;
using ATM.Api.RequestModels.Validators;
using ATM.UseCases.Account;
using ATM.UseCases.Account.GetBalance;
using ATM.UseCases.Account.Withdraw;
using ATM.UseCases.Transaction.GetTransactions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ATM.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/{cardNumber}")]
    [ApiController]
    public class OperationsController (IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        [Authorize]
        [Route("balance")]        
        [SwaggerOperation(
            Summary = "Gets the balance of an account.",
            Description = "This endpoint is used to get the balance of the account related to the given credit card number.",
            OperationId = "balance"
        )]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetBalanceResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBalance([FromRoute][SwaggerParameter("cardNumber", Description = "16 Digit Card Number with an asocciated account.")] string cardNumber,
            CancellationToken cancellationToken)
        {
            var cardNumberValidator = new CardNumberValidator();
            var cardNumberValidationResult = await cardNumberValidator.ValidateAsync(cardNumber, cancellationToken);

            if (!cardNumberValidationResult.IsValid)
                return BadRequest(cardNumberValidationResult.Errors.Select(e => e.ErrorMessage));

            var query = new GetBalanceCommand(cardNumber);
            var getBalanceResult = (await _mediator.Send(query, cancellationToken))
                                  .Match(resultValue => resultValue, error => error);

            if (getBalanceResult.Error != null)
                return Problem(getBalanceResult.Error.Message, null, getBalanceResult.Error.HttpStatusCode);
            else
                return Ok(getBalanceResult.Value);
        }

        [HttpPut]
        [Authorize]
        [Route("extraction")]
        [SwaggerOperation(
            Summary = "Withdraw funds of an account.",
            Description = "This endpoint is used to withdraw funds of the account related to the given credit card number. An error will be raised if there are insuficient funds.",
            OperationId = "extraction"
        )]
        [Produces("application/json")]
        [ProducesResponseType(typeof(WithdrawResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request,
            [FromRoute][SwaggerParameter("cardNumber", Description = "16 Digit Card Number with an asocciated account.")] string cardNumber,
            CancellationToken cancellationToken)
        {

            var modelValidator = new WithdrawRequestModelValidator();
            var cardNumberValidator = new CardNumberValidator();

            var modelValidationResult = await modelValidator.ValidateAsync(request, cancellationToken);
            var cardNumberValidationResult = await cardNumberValidator.ValidateAsync(cardNumber, cancellationToken);

            if (!modelValidationResult.IsValid || !cardNumberValidationResult.IsValid)
            {
                var errors = modelValidationResult.Errors.Concat(cardNumberValidationResult.Errors);
                return BadRequest(errors.Select(e => e.ErrorMessage));
            }

            var query = new WithdrawCommand(cardNumber, request.Amount);
            var withdrawResult = (await _mediator.Send(query, cancellationToken))
                                  .Match(resultValue => resultValue, error => error);

            if (withdrawResult.Error != null)
                return Problem(withdrawResult.Error.Message, null, withdrawResult.Error.HttpStatusCode);
            else
                return Ok(withdrawResult.Value);
        }

        [HttpGet]
        [Authorize]
        [Route("transactions")]
        [SwaggerOperation(
            Summary = "Gets the transactions of an account.",
            Description = "This endpoint is used to get the transactions of the account related to the given credit card number." +
            " It supports pagination via the pageSize and pageIndex parameters.",
            OperationId = "transactions"
        )]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetTransactionsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactions([SwaggerParameter("cardNumber", Description = "16 Digit Card Number with an asocciated account.")] string cardNumber,
            CancellationToken cancellationToken,
            [SwaggerParameter("pageSize", Description = "Size of results page, can't be less than 0.")] [FromQuery] int pageSize = 10,
            [SwaggerParameter("pageIndex", Description = "Page number of the results, can't be lass than 0.")][FromQuery] int pageIndex = 0)
        {
            var cardNumberValidator = new CardNumberValidator();
            var cardNumberValidationResult = await cardNumberValidator.ValidateAsync(cardNumber, cancellationToken);

            if (!cardNumberValidationResult.IsValid)            
                return BadRequest(cardNumberValidationResult.Errors.Select(e => e.ErrorMessage));            

            var query = new GetTransactionsCommand(cardNumber,pageIndex,pageSize);
            var transactionsResult = (await _mediator.Send(query, cancellationToken))
                                  .Match(resultValue => resultValue, error => error);

            if (transactionsResult.Error != null)
                return Problem(transactionsResult.Error.Message, null, transactionsResult.Error.HttpStatusCode);
            else
                return Ok(transactionsResult.Value);
        }
    }
}
