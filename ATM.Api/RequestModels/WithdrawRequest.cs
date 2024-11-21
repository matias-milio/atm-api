using System.ComponentModel;

namespace ATM.Api.RequestModels
{
    public class WithdrawRequest
    {
        [Description("Amount to withdraw.")]
        public decimal Amount { get; set; }
    }
}
