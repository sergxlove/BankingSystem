using BankingSystemCore.Models;
using BankingSystemDataAccess.Postgres.Abstractions;
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
    }
}
