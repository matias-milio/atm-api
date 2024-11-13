using MediatR;

namespace ATM.Core.Events
{
    public class CardBlockedEvent(int cardId) : INotification
    {
        public int CardId { get; set; } = cardId;
    }
}
