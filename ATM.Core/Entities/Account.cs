namespace ATM.Core.Entities
{
    public class Account : BaseEntity
    {
        public  string Number { get; set; }
        public  decimal Balance { get; set; }        
        public  int CardId { get; set; }
        public virtual  Card Card { get; set; }
    }   
}
