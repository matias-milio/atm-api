using ATM.Core;
using MediatR;

namespace ATM.UseCases.Account.GetBalance
{
    public class GetBalanceCommand(string cardNumber) : IRequest<Result<GetBalanceResponseDTO, Error>>
    {
        public string CardNumber { get; set; } = cardNumber;
    }
}
