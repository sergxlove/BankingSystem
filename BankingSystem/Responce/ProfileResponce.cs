using BankingSystemCore.Models;

namespace BankingSystem.Responce
{
    public class ProfileResponce
    {
        public Clients? Client {  get; set; }
        public List<Accounts> Accounts { get; set; } = [];
        public List<Deposits> Deposits { get; set; } = [];
        public List<Credits> Credits { get; set; } = [];
    }
}
