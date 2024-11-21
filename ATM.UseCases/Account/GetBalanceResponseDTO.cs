namespace ATM.UseCases.Account
{
    public record GetBalanceResponseDTO
    {
        public string CardHolderName { get; set; }
        public string AccountNumber { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime? LastExtractionDate { get; set; }
    }
}
