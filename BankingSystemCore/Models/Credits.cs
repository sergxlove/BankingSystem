namespace BankingSystemCore.Models
{
    public class Credits
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid AccountId { get; set; }
        public decimal SumCredit { get; set; }
        public int TermMonth { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal PaymentMonth { get; set; }
        public decimal LeftCredit { get; set; }
        public bool IsActive { get; set; }

        public static ResultModel<Credits> Create(Guid id, Guid clientId, Guid accountId, decimal sumCredit, int termMonth,
            DateOnly startDate, DateOnly endDate, decimal paymentMonth, decimal leftCredit,
            bool isActive)
        {
            return ResultModel<Credits>.Success(new Credits(id, clientId, accountId, sumCredit, termMonth, startDate, endDate, paymentMonth, leftCredit, isActive));
        }

        public static decimal GetPaymentsMonth(decimal sumCredit, int termMonth)
        {
            decimal percentMonth = sumCredit / 100;
            return decimal.Round(sumCredit / termMonth + percentMonth, 2);
        }

        private Credits(Guid id, Guid clientId, Guid accountId, decimal sumCredit, int termMonth,
            DateOnly startDate, DateOnly endDate, decimal paymentMonth, decimal leftCredit, 
            bool isActive)
        {
            Id = id;
            ClientId = clientId;
            AccountId = accountId;
            SumCredit = sumCredit;
            TermMonth = termMonth;
            StartDate = startDate;
            EndDate = endDate;
            PaymentMonth = paymentMonth;
            LeftCredit = leftCredit;
            IsActive = isActive;
        }
    }
}
