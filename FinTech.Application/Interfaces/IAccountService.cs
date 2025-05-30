using FinTech.Application.DTOs;
using FinTech.Domain.ValueObjects;

namespace FinTech.Application.Interfaces;

public interface IAccountService
{
    Task<AccountDto> CreateAccount(string ownerName, decimal initialBalance);
    Task<IEnumerable<AccountDto>> GetAllAccounts();
    Task<AccountDto> GetAccount(AccountNumber accountNumber);
}
