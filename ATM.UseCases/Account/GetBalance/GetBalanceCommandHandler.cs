using ATM.Core;
using ATM.Core.Contracts;
using ATM.Core.Entities;
using ATM.Core.Enums;
using ATM.Core.Errors;
using MediatR;

namespace ATM.UseCases.Account.GetBalance
{
    public class GetBalanceCommandHandler(IAtmRepository<Card> cardRepository) : IRequestHandler<GetBalanceCommand, Result<GetBalanceResponseDTO, Error>>
    {
        private readonly IAtmRepository<Card> _cardRepository = cardRepository;
        public async Task<Result<GetBalanceResponseDTO, Error>> Handle(GetBalanceCommand request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetByFilterAsync(filter: c => c.Number.Equals(request.CardNumber),
                includeProperties: "Account,CardHolder,Transactions");

            if (card == null)
                return GenericErrors.CardNotFound;

            if (card.Account == null)
                return GenericErrors.AccountNotFound;

            if (card.Status.Equals(CardStatus.Inactive))
                return GenericErrors.InactiveCard;

            var lastTransactionDate = card.Transactions.OrderByDescending(t => t.Date)?.FirstOrDefault()?.Date;

            return new GetBalanceResponseDTO { 
                AccountNumber = card.Account.Number,
                CardHolderName = card.CardHolder.Name,
                CurrentBalance = card.Account.Balance,
                LastExtractionDate = lastTransactionDate != null ? lastTransactionDate : null,
            };
        }
    }
}
