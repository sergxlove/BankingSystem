namespace BankingSystemCore.Models
{
    public class Managers
    {
        public Guid Id { get; set; }
        public string ClientSeries { get; set; } = string.Empty;
        public string ClientNumbers {  get; set; } = string.Empty;
        public string LoginManager {  get; set; } = string.Empty;

        public static ResultModel<Managers> Create(Guid id, string clientSeries, string clientNumbers, 
            string loginManager)
        {
            if (id == Guid.Empty)
                return ResultModel<Managers>.Failure("Manager ID cannot be empty");

            if (string.IsNullOrWhiteSpace(clientSeries))
                return ResultModel<Managers>.Failure("Client series is required");

            if (string.IsNullOrWhiteSpace(clientNumbers))
                return ResultModel<Managers>.Failure("Client numbers is required");

            if (string.IsNullOrWhiteSpace(loginManager))
                return ResultModel<Managers>.Failure("Login is required");

            return ResultModel<Managers>.Success(new Managers(id, clientSeries, clientNumbers, 
                loginManager));
        }

        private Managers(Guid id, string clientSeries, string clientNumbers, string loginManager)
        {
            Id = id;
            ClientSeries = clientSeries;
            ClientNumbers = clientNumbers;
            LoginManager = loginManager;
        }
    }
}
