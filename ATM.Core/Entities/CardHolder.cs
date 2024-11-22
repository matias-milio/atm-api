namespace ATM.Core.Entities
{
    public class CardHolder : BaseEntity
    {
        public string Name { get; set; }
        public string PIN { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
    }   
}
