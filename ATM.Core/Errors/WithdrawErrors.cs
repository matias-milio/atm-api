using System.Net;

namespace ATM.Core.Errors
{
    public class WithdrawErrors
    {
        public static readonly Error InsuficientFunds = new("AtmApi.InsuficientFunds", (int)HttpStatusCode.Forbidden, "Insuficient balance to withdraw.");
    }
}
