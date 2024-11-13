using System.Net;

namespace ATM.Core.Errors
{
    public class LoginErrors
    {
        public static readonly Error CardNotFound = new("AtmApi.CardNotFound", (int)HttpStatusCode.NotFound, "Card not found.");
        public static readonly Error IncorrectPIN = new("AtmApi.IncorrectPIN", (int)HttpStatusCode.Forbidden, "The given PIN was incorrect.");
        public static readonly Error CardBlocked = new("AtmApi.CardBlocked", (int)HttpStatusCode.Forbidden, "Card was blocked due to 4 incorrect PIN attempts.");
        public static readonly Error InactiveCard = new("AtmApi.InactiveCard", (int)HttpStatusCode.Forbidden, "The Given Card is inactive.");

    }
}
