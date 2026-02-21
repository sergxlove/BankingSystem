namespace BankingSystemDataAccess.Postgres.Models
{
    public class ManagersEntity
    {
        public Guid Id { get; set; }
        public string ClientSeries { get; set; } = string.Empty;
        public string ClientNumbers { get; set; } = string.Empty;
        public string LoginManager { get; set; } = string.Empty;
    }
}
