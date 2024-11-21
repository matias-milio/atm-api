using ATM.Core.Contracts;
using ATM.Core.Entities;
using ATM.Core.Events;
using MediatR;

namespace ATM.Core.EventHandlers
{
    public class TransactionCreatedEventHandler(IAtmRepository<Transaction> repository) : INotificationHandler<TransactionCreatedEvent>
    {
        private readonly IAtmRepository<Transaction> _repostitory = repository;      
        public async Task Handle(TransactionCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _repostitory.CreateAsync(new Transaction
            {
                Amount = notification.Amount,
                CardId = notification.CardId,
                Date = notification.Date
            });
        }
    }
}
