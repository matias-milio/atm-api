namespace ATM.UseCases.Account
{
    public record GetTransactionsDTO : PaginatedResponse
    {            
        public IEnumerable<TransactionDTO> Transactions { get; set; }   
    }

    public record TransactionDTO
    {
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
