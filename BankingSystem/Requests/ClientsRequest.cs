namespace BankingSystem.Requests
{
    public class ClientsRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string PassportSeries { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string AddressRegistration { get; set; } = string.Empty;
    }
}
