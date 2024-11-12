namespace ATM.Core.Entities
{
    public class CardHolder : BaseEntity
    {
        public required string Name { get; set; }
        public required string PIN { get; set; }
        public virtual required ICollection<Card> Cards { get; set; }
    }   
}
