using FinTech.Domain.Enums;
using FinTech.Domain.Exceptions;
using FinTech.Domain.ValueObjects;

namespace FinTech.Domain.Entities;

public class Account
{
    public AccountNumber Number { get; }
    public Money Balance { get; private set; }
    public string OwnerName { get; }
    public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();

    private readonly List<Transaction> _transactions = [];

    public Account(AccountNumber number, Money initialBalance, string ownerName)
    {
        Number = number ?? throw new ArgumentNullException(nameof(number));
        Balance = initialBalance ?? throw new ArgumentNullException(nameof(initialBalance));
        OwnerName = string.IsNullOrWhiteSpace(ownerName)
            ? throw new ArgumentException("Owner name cannot be empty", nameof(ownerName))
            : ownerName;

        if (initialBalance <= Money.Zero) 
            throw new InvalidTransactionException("Initial balance must be positive");
    }

    public void Deposit(Money amount)
    {
        if (amount <= Money.Zero)
            throw new InvalidTransactionException("Deposit amount must be positive");

        Balance += amount;
        _transactions.Add(new Transaction(Guid.NewGuid(), amount, TransactionType.Deposit, DateTime.UtcNow));
    }

    public void Withdraw(Money amount)
    {
        if (amount <= Money.Zero)
            throw new InvalidTransactionException("Withdrawal amount must be positive");

        if (Balance < amount)
            throw new InsufficientFundsException(Number.Value, Balance.Amount);

        Balance -= amount;
        _transactions.Add(new Transaction(Guid.NewGuid(), amount, TransactionType.Withdrawal, DateTime.UtcNow));
    }

    public void Transfer(Account targetAccount, Money amount)
    {
        ArgumentNullException.ThrowIfNull(targetAccount);
        Withdraw(amount);
        targetAccount.Deposit(amount);
        _transactions.Add(new Transaction(Guid.NewGuid(), amount, TransactionType.TransferIn, DateTime.UtcNow));
    }
}
