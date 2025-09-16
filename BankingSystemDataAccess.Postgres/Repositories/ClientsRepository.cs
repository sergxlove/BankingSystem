using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystemDataAccess.Postgres.Repositories
{
    public class ClientsRepository
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
                    FirstName = client.FirstName,
                    SecondName = client.LastName,
                    LastName = client.LastName,
                    BirthDate = client.BirthDate,
                    PassportSeries = client.PassportSeries,
                    PassportNumber = client.PassportNumber,
                    PhoneNumber = client.PhoneNumber,
                    EmailAddress = client.EmailAddress,
                    AddressRegistration = client.AddressRegistration,
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
                .ExecuteUpdateAsync(s => s.SetProperty(s => s.FirstName, clients.FirstName)
                .SetProperty(s => s.SecondName, clients.SecondName)
                .SetProperty(s => s.LastName, clients.LastName)
                .SetProperty(s => s.BirthDate, clients.BirthDate)
                .SetProperty(s => s.PassportSeries, clients.PassportSeries)
                .SetProperty(s => s.PassportNumber, clients.PassportNumber)
                .SetProperty(s => s.PhoneNumber, clients.PhoneNumber)
                .SetProperty(s => s.EmailAddress, clients.EmailAddress)
                .SetProperty(s => s.AddressRegistration, clients.AddressRegistration)
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
                .FirstOrDefaultAsync(a => a.PassportSeries == passportSeries
                    && a.PassportNumber == passportNumber);
            if (client is null) return Guid.Empty;
            return client.Id;
        }

        public async Task<Clients?> GetAsync(string passportSeries, string passportNumber, 
            CancellationToken token)
        {
            var client = await _context.Clients
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.PassportSeries == passportSeries
                    && a.PassportNumber == passportNumber);
            if (client is null) return null;
            return Clients.Create(client.Id, client.FirstName, client.SecondName, client.LastName,
                client.BirthDate, client.PassportSeries, client.PassportNumber, client.PhoneNumber,
                client.EmailAddress, client.AddressRegistration, client.DateRegistration).Value!;
        }
    }
}
