using System.Reflection.Metadata;

namespace BankingSystemDataAccess.Postgres.Models
{
    public class SystemTableEntity
    {
        public int Id { get; set; } 
        public string NumberCardLast { get; set; } = string.Empty;    

    }
}
