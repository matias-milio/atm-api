using ATM.Core;
using MediatR;

namespace ATM.UseCases.Account.Withdraw
{
    public class WithdrawCommand(string cardNumber, decimal amount) : IRequest<Result<WithdrawResponseDTO, Error>>
    {
        public string CardNumber { get; set; } = cardNumber;
        public decimal Amount { get; set; } = amount;
    }
}
