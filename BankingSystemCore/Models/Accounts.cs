using System.Security.Principal;

namespace BankingSystemCore.Models
{
    public class Accounts
    {
        public Guid Id { get; set; }
        public Guid ClientsId { get; set; }
        public string AccountType { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public DateOnly OpenDate { get; set; }
        public DateOnly CloseDate { get; set; }
        public bool IsActive { get; set; }

        public static ResultModel<Accounts> Create(Guid id, Guid clientsId, string accountType, string accountNumber,
            decimal balance, string currencyCode, DateOnly openDate, DateOnly closeDate,
            bool isActive)
        {
            if (id == Guid.Empty)
                return ResultModel<Accounts>.Failure("Account ID cannot be empty");

            if (clientsId == Guid.Empty)
                return ResultModel<Accounts>.Failure("Client ID cannot be empty");

            if (string.IsNullOrWhiteSpace(accountType))
                return ResultModel<Accounts>.Failure("Account type is required");

            if (string.IsNullOrWhiteSpace(accountNumber))
                return ResultModel<Accounts>.Failure("Account number is required");

            if (string.IsNullOrWhiteSpace(currencyCode))
                return ResultModel<Accounts>.Failure("Currency code is required");
            return ResultModel<Accounts>.Success(new Accounts(id, clientsId, accountType,
                accountNumber, balance, currencyCode, openDate, closeDate, isActive));  
        }

        private Accounts(Guid id, Guid clientsId, string accountType, string accountNumber,
            decimal balance, string currencyCode, DateOnly openDate, DateOnly closeDate,
            bool isActive)
        {
            Id = id;
            ClientsId = clientsId;
            AccountType = accountType;
            AccountNumber = accountNumber;
            Balance = balance;
            CurrencyCode = currencyCode;
            OpenDate = openDate;
            CloseDate = closeDate;
            IsActive = isActive;
        }
    }
}
