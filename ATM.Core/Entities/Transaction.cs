
namespace ATM.Core.Entities
{
    public class Transaction : BaseEntity
    {
        public required decimal Amount { get; set; }
        public required DateTime Date {  get; set; }
        public required int CardId { get; set; }
        public virtual required Card Card { get; set; }
    }

}
