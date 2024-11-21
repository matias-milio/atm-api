using ATM.Core.Contracts;
using ATM.Core.Entities;
using ATM.Core;
using ATM.Core.Enums;
using ATM.Core.Errors;
using ATM.Core.Events;
using MediatR;

namespace ATM.UseCases.Account.Withdraw
{
    public class WithdrawCommandHandler(IAtmRepository<Card> cardRepository, IMediator _mediator,
        IAtmRepository<Core.Entities.Account> accountRepository) : IRequestHandler<WithdrawCommand, Result<WithdrawResponseDTO, Error>>
    {
        private readonly IAtmRepository<Card> _cardRepository = cardRepository;
        private readonly IAtmRepository<Core.Entities.Account> _accountRepository = accountRepository;
        public async Task<Result<WithdrawResponseDTO, Error>> Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetByFilterAsync(filter: c => c.Number.Equals(request.CardNumber),
                includeProperties: "Account,Transactions");

            if (card == null)
                return GenericErrors.CardNotFound;

            if (card.Account == null)
                return GenericErrors.AccountNotFound;

            if (card.Status.Equals(CardStatus.Inactive))
                return GenericErrors.InactiveCard;

            if (card.Account.Balance < request.Amount)
                return WithdrawErrors.InsuficientFunds;

            var account = card.Account;
            account.Balance -= request.Amount;
            await _accountRepository.UpdateAsync(account);

            var transactionDate = DateTime.UtcNow;            
            await _mediator.Publish(new TransactionCreatedEvent(request.Amount, transactionDate, card.Id), cancellationToken);
                      
            return new WithdrawResponseDTO {
                CardNumber = card.Number,
                Amount = request.Amount,
                Date = transactionDate,
                NewBalance = account.Balance
            };
        }
    }
}
