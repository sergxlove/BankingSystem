using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;
using BankingSystemDataAccess.Postgres.Dto;
using BankingSystemDataAccess.Postgres.Models;
using BankingSystemDataAccess.Postgres.Services;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemDataAccess.Postgres.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly BankingSystemDbContext _context;

        public ClientsRepository(BankingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(Clients client, CancellationToken token)
        {
            try
            {
                ClientsEntity clientEntity = new ClientsEntity()
                {
                    Id = client.Id,
                    FirstName = SimpleEncryptionService.Encrypt(client.FirstName),
                    SecondName = SimpleEncryptionService.Encrypt(client.LastName),
                    LastName = SimpleEncryptionService.Encrypt(client.LastName),
                    BirthDate = client.BirthDate,
                    PassportSeries = SimpleEncryptionService.Encrypt(client.PassportSeries),
                    PassportNumber = SimpleEncryptionService.Encrypt(client.PassportNumber),
                    PhoneNumber = SimpleEncryptionService.Encrypt(client.PhoneNumber),
                    EmailAddress = SimpleEncryptionService.Encrypt(client.EmailAddress),
                    AddressRegistration = SimpleEncryptionService.Encrypt(client.AddressRegistration),
                    DateRegistration = client.DateRegistration,
                };
                await _context.Clients.AddAsync(clientEntity, token);
                await _context.SaveChangesAsync();
                return clientEntity.Id;
            }
            catch
            {
                return Guid.Empty;
            }
        }

        public async Task<int> UpdateAsync(Clients clients, CancellationToken token)
        {
            return await _context.Clients
                .AsNoTracking()
                .Where(a => a.Id == clients.Id)
                .ExecuteUpdateAsync(s => s.SetProperty(s => s.FirstName, SimpleEncryptionService.Encrypt(clients.FirstName))
                .SetProperty(s => s.SecondName, SimpleEncryptionService.Encrypt(clients.SecondName))
                .SetProperty(s => s.LastName, SimpleEncryptionService.Encrypt(clients.LastName))
                .SetProperty(s => s.BirthDate, clients.BirthDate)
                .SetProperty(s => s.PassportSeries, SimpleEncryptionService.Encrypt(clients.PassportSeries))
                .SetProperty(s => s.PassportNumber, SimpleEncryptionService.Encrypt(clients.PassportNumber))
                .SetProperty(s => s.PhoneNumber, SimpleEncryptionService.Encrypt(clients.PhoneNumber))
                .SetProperty(s => s.EmailAddress, SimpleEncryptionService.Encrypt(clients.EmailAddress))
                .SetProperty(s => s.AddressRegistration, SimpleEncryptionService.Encrypt(clients.AddressRegistration))
                .SetProperty(s => s.DateRegistration, clients.DateRegistration), token);
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken token)
        {
            return await _context.Clients
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(token);
        }

        public async Task<Guid> GetIdAsync(string passportSeries, string passportNumber,
            CancellationToken token)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.PassportSeries == SimpleEncryptionService.Encrypt(passportSeries)
                    && a.PassportNumber == SimpleEncryptionService.Encrypt(passportNumber));
            if (client is null) return Guid.Empty;
            return client.Id;
        }

        public async Task<Clients?> GetAsync(string passportSeries, string passportNumber,
            CancellationToken token)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.PassportSeries == SimpleEncryptionService.Encrypt(passportSeries)
                    && a.PassportNumber == SimpleEncryptionService.Encrypt(passportNumber));
            if (client is null) return null;
            return Clients.Create(client.Id, SimpleEncryptionService.Decrypt(client.FirstName),
                SimpleEncryptionService.Decrypt(client.SecondName),
                SimpleEncryptionService.Decrypt(client.LastName),
                client.BirthDate,
                SimpleEncryptionService.Decrypt(client.PassportSeries),
                SimpleEncryptionService.Decrypt(client.PassportNumber),
                SimpleEncryptionService.Decrypt(client.PhoneNumber),
                SimpleEncryptionService.Decrypt(client.EmailAddress),
                SimpleEncryptionService.Decrypt(client.AddressRegistration),
                client.DateRegistration).Value!;
        }

        public async Task<bool> CheckAsync(string passportSeries, string passportNumber,
            CancellationToken token)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.PassportSeries == SimpleEncryptionService.Encrypt(passportSeries)
                    && a.PassportNumber == SimpleEncryptionService.Encrypt(passportNumber));
            if (client is null) return false;
            return true;
        }

        public async Task<List<ClientBalanceDto>> GetClientsByBalanceRangeAsync
            (decimal minBalance, decimal maxBalance, CancellationToken token)
        {
            var query = from client in _context.Clients
                        join account in _context.Accounts on client.Id equals account.ClientsId into accounts
                        select new
                        {
                            FullName = client.LastName + " " + client.FirstName + " " + client.SecondName,
                            TotalBalance = accounts.Sum(a => a.Balance)
                        };
            var result = await query
                .Where(x => x.TotalBalance >= minBalance && x.TotalBalance <= maxBalance)
                .Select(x => new ClientBalanceDto
                {
                    FullName = x.FullName,
                    Balance = x.TotalBalance
                })
                .OrderByDescending(x => x.Balance)
                .ToListAsync(token);

            return result;
        }

        public async Task<List<ClientCreditDto>> GetBorrowersByMonthsLeftAsync(int maxMonthsLeft,
            CancellationToken token)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            // PostgreSQL: вычисляем разницу в месяцах между сегодня и датой окончания
            var query = from credit in _context.Credits
                        join client in _context.Clients on credit.ClientId equals client.Id
                        where credit.IsActive && credit.EndDate >= today
                        //let monthsLeft = EF.Functions.DateDiffMonth(today, credit.EndDate)
                        //where monthsLeft < maxMonthsLeft
                        select new ClientCreditDto
                        {
                            FullName = client.LastName + " " + client.FirstName + " " + client.SecondName,
                            LeftCredit = credit.LeftCreadit
                        };

            var result = await query
                .OrderBy(x => x.LeftCredit)
                .ToListAsync();

            return result;
        }
    }
}
