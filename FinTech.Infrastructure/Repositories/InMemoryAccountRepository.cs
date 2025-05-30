
using FinTech.Application.Interfaces;
using FinTech.Domain.Entities;
using FinTech.Domain.Exceptions;
using FinTech.Domain.ValueObjects;

namespace FinTech.Infrsdtructure.Repositories;

public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<AccountNumber, Account> _accounts = [];

    public Task<Account> GetByIdAsync(AccountNumber accountNumber)
    {
        if (_accounts.TryGetValue(accountNumber, out var account))
            return Task.FromResult(account);

        throw new AccountNotFoundException(accountNumber.Value);
    }

    public Task<IEnumerable<Account>> GetAllAsync()
    {
        return Task.FromResult(_accounts.Select(x=>x.Value));
    }

    public Task AddAsync(Account account)
    {
        if (_accounts.ContainsKey(account.Number))
            throw new InvalidOperationException($"Account {account.Number} already exists");

        _accounts.Add(account.Number, account);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Account account)
    {
        if (!_accounts.ContainsKey(account.Number))
            throw new AccountNotFoundException(account.Number.Value);

        _accounts[account.Number] = account;
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(AccountNumber accountNumber)
        => Task.FromResult(_accounts.ContainsKey(accountNumber));
}
