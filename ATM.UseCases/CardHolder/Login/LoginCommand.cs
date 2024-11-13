using ATM.Core;
using MediatR;

namespace ATM.UseCases.CardHolder.Login
{
    public class LoginCommand : IRequest<Result<string, Error>>
    {
        public LoginCommand(string cardNumber, string PIN)
        {
            CardNumber = cardNumber;
            PIN = PIN;
        }

        public string CardNumber { get; set; }
        public string PIN { get; set; }
    }

}
