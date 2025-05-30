
using FinTech.Application.DTOs;
using FinTech.Domain.Entities;
using FinTech.Domain.Enums;
using FinTech.Domain.ValueObjects;

namespace FinTech.Application.Interfaces;

public interface ITransactionService
{
    Task DepositAsync(AccountNumber accountNumber, decimal amount);
    Task WithdrawAsync(AccountNumber accountNumber, decimal amount);
    Task TransferAsync(AccountNumber sourceAccountNumber, AccountNumber targetAccountNumber, decimal amount);
    Task<IEnumerable<TransactionDto>> GetTransactionsAsync(AccountNumber accountNumber, string? type = null, DateTime? fromDate = null, DateTime? toDate = null);
}
