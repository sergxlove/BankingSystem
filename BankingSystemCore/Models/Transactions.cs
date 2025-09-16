namespace BankingSystemCore.Models
{
    public class Transactions
    {
        public Guid Id { get; set; }
        public Guid ProducerAccount { get; set; }
        public Guid ConsumerAccount { get; set; }
        public Guid TypeOperation { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateOnly DateCreated { get; set; }

        public static ResultModel<Transactions> Create(Guid id, Guid producerAccount, 
            Guid consumerAccount, Guid typeOperation, decimal amount, string description, 
            DateOnly dateCreated)
        {
            return ResultModel<Transactions>.Success(new Transactions(id, producerAccount,
                consumerAccount, typeOperation, amount, description, dateCreated));
        }
        private Transactions(Guid id, Guid producerAccount, Guid consumerAccount, 
            Guid typeOperation, decimal amount, string description, DateOnly dateCreated)
        {
            Id = id;
            ProducerAccount = producerAccount;
            ConsumerAccount = consumerAccount;
            TypeOperation = typeOperation;
            Amount = amount;
            Description = description;
            DateCreated = dateCreated;
        }
    }
}
