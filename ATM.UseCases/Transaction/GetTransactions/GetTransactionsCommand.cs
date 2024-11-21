using ATM.Core;
using ATM.UseCases.Account;
using MediatR;

namespace ATM.UseCases.Transaction.GetTransactions
{
    public class GetTransactionsCommand : PaginatedRequest, IRequest<Result<GetTransactionsDTO, Error>>
    {
        public string CardNumber { get; set; }
        public GetTransactionsCommand(string cardNumber,int pageIndex, int pageSize) : base(pageIndex, pageSize)
        {
            CardNumber = cardNumber;           
        }       
    }
}
