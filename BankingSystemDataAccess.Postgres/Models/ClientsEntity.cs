namespace BankingSystemDataAccess.Postgres.Models
{
    public class ClientsEntity
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty; 
        public string SecondName {  get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string PassportSeries { get; set; } = string.Empty;
        public string PasswordNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string AddressRegistration {  get; set; } = string.Empty;
        public DateOnly DateRegistration { get; set; }

        public virtual List<AccountsEntity> Accounts { get; set; } = new();
        public virtual List<DepositsEntity> Deposits { get; set; } = new();
        public virtual List<CreditsEntity> Credits { get; set; } = new();

    }
}
