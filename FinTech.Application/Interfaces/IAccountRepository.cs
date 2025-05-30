using FinTech.Domain.Entities;
using FinTech.Domain.ValueObjects;

namespace FinTech.Application.Interfaces;

public interface IAccountRepository
{
    Task<Account> GetByIdAsync(AccountNumber accountNumber);
    Task<IEnumerable<Account>> GetAllAsync();
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
    Task<bool> ExistsAsync(AccountNumber accountNumber);
}
