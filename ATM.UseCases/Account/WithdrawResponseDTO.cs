namespace ATM.UseCases.Account
{
    public record WithdrawResponseDTO
    {
       public decimal  Amount { get; set; }  
       public decimal NewBalance { get; set; }
       public DateTime Date { get; set; }
       public string CardNumber { get; set; }
    }
}
