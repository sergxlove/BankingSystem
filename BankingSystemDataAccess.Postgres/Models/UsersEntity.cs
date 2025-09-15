namespace BankingSystemDataAccess.Postgres.Models
{
    public class UsersEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
        public string Role {  get; set; } = string.Empty;
    }
}
