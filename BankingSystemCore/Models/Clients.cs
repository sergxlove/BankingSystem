namespace BankingSystemCore.Models
{
    public class Clients
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string SecondName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string PassportSeries { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string AddressRegistration { get; set; } = string.Empty;
        public DateOnly DateRegistration { get; set; }

        public static ResultModel<Clients> Create(Guid id, string firstName, string secondName, 
            string lastName, DateOnly birthDate, string passportSeries, string passportNumber, 
            string phoneNumber,  string emailAddress, string addressRegistration,
            DateOnly dateRegistration)
        {
            if (id == Guid.Empty)
                return ResultModel<Clients>.Failure("Client ID cannot be empty");

            if (string.IsNullOrWhiteSpace(firstName))
                return ResultModel<Clients>.Failure("First name is required");

            if (string.IsNullOrWhiteSpace(lastName))
                return ResultModel<Clients>.Failure("Last name is required");

            if (string.IsNullOrWhiteSpace(passportSeries))
                return ResultModel<Clients>.Failure("Passport series is required");

            if (string.IsNullOrWhiteSpace(passportNumber))
                return ResultModel<Clients>.Failure("Passport number is required");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                return ResultModel<Clients>.Failure("Phone number is required");

            if (string.IsNullOrWhiteSpace(addressRegistration))
                return ResultModel<Clients>.Failure("Registration address is required");

            return ResultModel<Clients>.Success(new Clients(id, firstName, secondName, lastName,
                birthDate, passportSeries, passportNumber, phoneNumber, emailAddress, 
                addressRegistration, dateRegistration));
        }

        private Clients(Guid id, string firstName, string secondName, string lastName, 
            DateOnly birthDate, string passportSeries, string passportNumber, string phoneNumber, 
            string emailAddress, string addressRegistration, DateOnly dateRegistration)
        {
            Id = id;
            FirstName = firstName;
            SecondName = secondName;
            LastName = lastName;
            BirthDate = birthDate;
            PassportSeries = passportSeries;
            PassportNumber = passportNumber;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            AddressRegistration = addressRegistration;
            DateRegistration = dateRegistration;
        }
    }
}
