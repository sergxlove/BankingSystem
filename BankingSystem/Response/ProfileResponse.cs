using BankingSystemCore.Models;

namespace BankingSystem.Response
{
    public class ProfileResponse
    {
        public Clients? Client {  get; set; }
        public List<Accounts> Accounts { get; set; } = [];
        public List<Deposits> Deposits { get; set; } = [];
        public List<Credits> Credits { get; set; } = [];
    }
}
