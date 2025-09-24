namespace BankingSystemCore.Models
{
    public class Deposits
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid AccountId { get; set; }
        public decimal SumDeposit { get; set; }
        public int TermMonth { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int PercentYear { get; set; }
        public bool IsActive { get; set; }


        public static ResultModel<Deposits> Create(Guid id, Guid clientId, Guid accountId, 
            decimal sumDeposit, int termMonth, DateOnly startDate, DateOnly endDate,
            int percentYear, bool isActive)
        {
            return ResultModel<Deposits>.Success(new Deposits(id, clientId, accountId, sumDeposit, 
                termMonth, startDate, endDate, percentYear, isActive));
        }

        public static int GetCurrentPercentYear()
        {
            return 15;
        }
        private Deposits(Guid id, Guid clientId, Guid accountId, decimal sumDeposit, int termMonth,
            DateOnly startDate, DateOnly endDate, int percentYear, bool isActive)
        {
            Id = id;
            ClientId = clientId;
            AccountId = accountId;
            SumDeposit = sumDeposit;
            TermMonth = termMonth;
            StartDate = startDate;
            EndDate = endDate;
            PercentYear = percentYear;
            IsActive = isActive;
        }
    }
}
