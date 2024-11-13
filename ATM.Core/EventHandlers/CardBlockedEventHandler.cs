using ATM.Core.Contracts;
using ATM.Core.Entities;
using ATM.Core.Enums;
using ATM.Core.Events;
using MediatR;

namespace ATM.Core.EventHandlers
{
    public class CardBlockedEventHandler(IAtmRepository<Card> repository) : INotificationHandler<CardBlockedEvent>
    {
        private readonly IAtmRepository<Card> _repostitory = repository;
        public async Task Handle(CardBlockedEvent notification, CancellationToken cancellationToken)
        {
            var card = await repository.GetByFilterAsync(filter: c => c.Id.Equals(notification.CardId));
            card.Status = CardStatus.Inactive;
            await _repostitory.UpdateAsync(card);
        }
    }
}
