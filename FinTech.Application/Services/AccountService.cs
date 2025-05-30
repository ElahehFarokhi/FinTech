
using FinTech.Application.Interfaces;
using FinTech.Domain.Entities;
using FinTech.Application.DTOs;
using FinTech.Domain.ValueObjects;

namespace FinTech.Application.Services;

public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    public async Task<AccountDto> CreateAccount(string ownerName, decimal initialBalance)
    {
        var accountNumber = AccountNumber.GenerateRandom();
        var account = new Account(
            accountNumber,
            new Money(initialBalance),
            ownerName);

        await accountRepository.AddAsync(account);
        return AccountDto.FromDomain(account);
    }
    
    public async Task<IEnumerable<AccountDto>> GetAllAccounts()
    {
        var accounts = await accountRepository.GetAllAsync();

        return accounts.Select(account => AccountDto.FromDomain(account));
    }

    public async Task<AccountDto> GetAccount(AccountNumber accountNumber)
    {
        var account = await accountRepository.GetByIdAsync(accountNumber);

        return AccountDto.FromDomain(account);
    }

}