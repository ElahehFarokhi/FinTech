using FinTech.Application.DTOs;
using FinTech.Domain.ValueObjects;

namespace FinTech.Application.Interfaces;

public interface IAccountService
{
    Task<AccountDto> CreateAccount(string ownerName, decimal initialBalance);
    Task<IEnumerable<AccountDetailsDto>> GetAllAccounts();
    Task<AccountDetailsDto> GetAccount(AccountNumber accountNumber);
}
