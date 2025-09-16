namespace BankingSystemCore.Models
{
    public class OperationsTransactions
    {
        public Guid Id { get; set; }
        public string TypeOperation { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public static ResultModel<OperationsTransactions> Create(Guid id,  
            string typeOperation, string description)
        {
            return ResultModel<OperationsTransactions>.Success(new OperationsTransactions(id,
                typeOperation, description));
        }
        private OperationsTransactions(Guid id, string typeOperation, string description)
        { 
            Id = id;
            TypeOperation = typeOperation;
            Description = description;
        }
    }
}
