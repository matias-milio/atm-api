using ATM.Core;
using ATM.Core.Contracts;
using ATM.Core.Entities;
using ATM.Core.Enums;
using ATM.Core.Errors;
using ATM.UseCases.Account;
using MediatR;
using System.Transactions;

namespace ATM.UseCases.Transaction.GetTransactions
{
    public class GetTransactionsHandler(IAtmRepository<Card> cardRepository,IAtmRepository<Core.Entities.Transaction> transactionRepository)
        : IRequestHandler<GetTransactionsCommand, Result<GetTransactionsDTO, Error>>
    {
        private readonly IAtmRepository<Core.Entities.Transaction> _transactionRepository = transactionRepository;
        private readonly IAtmRepository<Card> _cardRepository = cardRepository;

        public async Task<Result<GetTransactionsDTO, Error>> Handle(GetTransactionsCommand request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetByFilterAsync(filter: c => c.Number.Equals(request.CardNumber));

            if (card == null)
                return GenericErrors.CardNotFound;            

            if (card.Status.Equals(CardStatus.Inactive))
                return GenericErrors.InactiveCard;

            var transactions = await _transactionRepository.GetPaginatedAsync(filter: t => t.CardId.Equals(card.Id),
                                                            pageSize: request.PageSize, pageIndex: request.PageIndex);

            return new GetTransactionsDTO
            {
                Transactions = transactions.Select(t => new TransactionDTO { Amount = t.Amount, TransactionDate = t.Date}),
                CurrentPage = transactions.CurrentPage,
                TotalPages = transactions.TotalPages,
                PageSize = transactions.PageSize,
                TotalCount = transactions.TotalCount
            };
        }
    }
}
