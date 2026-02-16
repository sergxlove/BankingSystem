namespace BankingSystemDataAccess.Postgres.Dto
{
    public class ClientBalanceDto
    {
        public string FullName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
