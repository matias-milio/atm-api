
namespace ATM.Core.Entities
{
    public class Transaction : BaseEntity
    {
   
        public  decimal Amount { get; set; }
        public  DateTime Date {  get; set; }
        public  int CardId { get; set; }
        public virtual Card Card { get; set; }
    }

}
