using ATM.Core.Enums;

namespace ATM.Core.Entities
{
    public class Card : BaseEntity
    {
        public string Number { get; set; }
        public CardStatus Status { get; set; }
        public int CardHolderId { get; set; }
        public virtual CardHolder CardHolder { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }

        public Card(int id, string number, CardStatus status)
        {
            Id = id;
            Number = number;
            Status = status;
        }
    }    
}
