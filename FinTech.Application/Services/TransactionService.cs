using FinTech.Application.DTOs;
using FinTech.Application.Interfaces;
using FinTech.Domain.Entities;
using FinTech.Domain.Enums;
using FinTech.Domain.Exceptions;
using FinTech.Domain.ValueObjects;

namespace FinTech.Application.Services;

public class TransactionService(IAccountRepository accountRepository, ITransactionRepository transactionRepository) : ITransactionService
{
    public async Task DepositAsync(AccountNumber accountNumber, decimal amount)
    {
        var account = await accountRepository.GetByIdAsync(accountNumber)
                 ?? throw new AccountNotFoundException(accountNumber.Value);

        account.Deposit(new Money(amount));
        await accountRepository.UpdateAsync(account);

        var transaction = new Transaction(
            Guid.NewGuid(),
            new Money(amount),
            TransactionType.Deposit,
            DateTime.UtcNow,
            accountNumber);

        await transactionRepository.AddAsync(transaction);
    }

    public async Task WithdrawAsync(AccountNumber accountNumber, decimal amount)
    {
        var account = await accountRepository.GetByIdAsync(accountNumber)
            ?? throw new AccountNotFoundException(accountNumber.Value);

        var money = new Money(amount);
        account.Withdraw(money);

        await accountRepository.UpdateAsync(account);

        var transaction = new Transaction(
            Guid.NewGuid(),
            money,
            TransactionType.Withdrawal,
            DateTime.UtcNow,
            accountNumber);

        await transactionRepository.AddAsync(transaction);
    }

    public async Task TransferAsync(
            AccountNumber sourceAccountNumber,
            AccountNumber targetAccountNumber,
            decimal amount)
    {

        var sourceAccount = await accountRepository.GetByIdAsync(sourceAccountNumber)
?? throw new AccountNotFoundException(sourceAccountNumber.Value);

        var targetAccount = await accountRepository.GetByIdAsync(targetAccountNumber)
            ?? throw new AccountNotFoundException(targetAccountNumber.Value);

        var money = new Money(amount);
        try
        {
            sourceAccount.Withdraw(money);
            targetAccount.Deposit(money);

            await accountRepository.UpdateAsync(sourceAccount);
            await accountRepository.UpdateAsync(targetAccount);

            var transaction = new Transaction(
                Guid.NewGuid(),
                money,
                TransactionType.TransferOut,
                DateTime.UtcNow,
                sourceAccountNumber,
                targetAccountNumber);

            await transactionRepository.AddAsync(transaction);
        }
        catch (Exception)
        {
            await RollbackTransaction(sourceAccount, targetAccount, money);
            throw;
        }
    }

    public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(AccountNumber accountNumber, string? type = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var transactions = await transactionRepository.GetByFiltersAsync(accountNumber, type, fromDate, toDate);
        
        return transactions
            .OrderByDescending(t => t.Timestamp)
            .Select(TransactionDto.FromDomain);
    }

    private async Task RollbackTransaction(Account sourceAccount, Account targetAccount, Money amount)
    {
        sourceAccount.Deposit(amount); // Revert the withdrawal
        await accountRepository.UpdateAsync(sourceAccount);

        targetAccount.Withdraw(amount); // Revert the deposit
        await accountRepository.UpdateAsync(targetAccount);
    }
}
