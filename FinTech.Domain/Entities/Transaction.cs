using FinTech.Domain.Enums;
using FinTech.Domain.ValueObjects;

namespace FinTech.Domain.Entities;

public class Transaction
{
    public Guid Id { get; }
    public Money Balance { get; }
    public TransactionType Type { get; }
    public DateTime Timestamp { get; }
    public AccountNumber? SourceAccount { get; }
    public AccountNumber? DestinationAccount { get; }

    public Transaction(
        Guid id,
        Money amount,
        TransactionType type,
        DateTime timestamp,
        AccountNumber? sourceAccount = null,
        AccountNumber? destinationAccount = null)
    {
        Id = id;
        Balance = amount ?? throw new ArgumentNullException(nameof(amount));
        Type = type;
        Timestamp = timestamp;
        SourceAccount = sourceAccount;
        DestinationAccount = destinationAccount;
    }
}
