namespace ATM.Core.Entities
{
    public class Account : BaseEntity
    {
        public required string Number { get; set; }
        public decimal Balance { get; set; }
        public virtual required Card Card { get; set; }
    }   
}
