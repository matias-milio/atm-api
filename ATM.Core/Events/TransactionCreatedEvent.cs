using MediatR;

namespace ATM.Core.Events
{
    public class TransactionCreatedEvent(decimal amount, DateTime date, int cardId) : INotification
    {
        public decimal Amount { get; set; } = amount;
        public DateTime Date { get; set; } = date;
        public int CardId { get; set; } = cardId;
    }
}
