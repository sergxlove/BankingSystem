using BankingSystemDataAccess.Postgres.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemDataAccess.Postgres.Repositories
{
    public class SystemTableRepository : ISystemTableRepository
    {
        private readonly BankingSystemDbContext _context;

        public SystemTableRepository(BankingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetAndIncrementAsync()
        {
            var data = await _context.SystemTable
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == 1);
            if (data is null) return string.Empty;
            long numberCard = Convert.ToInt64(data.NumberCardLast);
            numberCard++;
            string numberCardStr = Convert.ToString(numberCard);
            if (numberCardStr.Length > 16) return string.Empty;
            data.NumberCardLast = numberCardStr;
            await _context.SystemTable
                .AsNoTracking()
                .Where(a => a.Id == 1)
                .ExecuteUpdateAsync(s => s.SetProperty(s => s.NumberCardLast, numberCardStr));
            return numberCardStr;
        }
    }
}
