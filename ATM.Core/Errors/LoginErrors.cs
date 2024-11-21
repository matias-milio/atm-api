using System.Net;

namespace ATM.Core.Errors
{
    public class LoginErrors
    {
        public static readonly Error IncorrectPIN = new("AtmApi.IncorrectPIN", (int)HttpStatusCode.Forbidden, "The given PIN was incorrect.");
        public static readonly Error CardBlocked = new("AtmApi.CardBlocked", (int)HttpStatusCode.Forbidden, "Card was blocked due to 3 (three) incorrect PIN attempts.");

    }
}
