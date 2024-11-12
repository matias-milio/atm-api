using ATM.Core.Enums;

namespace ATM.Core.Entities
{
    public class Card : BaseEntity
    {
        public required string Number { get; set; }
        public required CardStatus Status { get; set; }
        public required int CardHolderId { get; set; }
        public virtual required CardHolder CardHolder { get; set; }
        public virtual required Account Account { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }    
}
