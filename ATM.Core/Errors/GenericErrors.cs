using System.Net;

namespace ATM.Core.Errors
{
    public class GenericErrors
    {
        public static readonly Error CardNotFound = new("AtmApi.CardNotFound", (int)HttpStatusCode.NotFound, "Card not found.");
        public static readonly Error InactiveCard = new("AtmApi.InactiveCard", (int)HttpStatusCode.Forbidden, "The Given Card is inactive.");
        public static readonly Error AccountNotFound = new("AtmApi.AccountNotFound", (int)HttpStatusCode.NotFound, "Account not found.");

    }
}
