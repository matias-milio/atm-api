namespace ATM.Core.Entities
{
    public class Account : BaseEntity
    {
        public required string Number { get; set; }
        public required decimal Balance { get; set; }        
        public required int CardId { get; set; }
        public virtual required Card Card { get; set; }
    }   
}
