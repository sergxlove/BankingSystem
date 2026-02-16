namespace BankingSystemDataAccess.Postgres.Dto
{
    public class ClientCreditDto
    {
        public string FullName { get; set; } = string.Empty;
        public decimal LeftCredit { get; set; }
    }
}
