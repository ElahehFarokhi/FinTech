using FinTech.Domain.Entities;

namespace FinTech.Application.DTOs;

public record TransactionDto
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public string Type { get; init; }
    public DateTime Timestamp { get; init; }
    public string SourceAccount { get; init; }
    public string? DestinationAccount { get; init; }

    public static TransactionDto FromDomain(Transaction transaction) => new()
    {
        Id = transaction.Id,
        Amount = transaction.Balance.Amount,
        Type = transaction.Type.ToString(),
        Timestamp = transaction.Timestamp,
        SourceAccount = transaction.SourceAccount?.Value,
        DestinationAccount = transaction.DestinationAccount?.Value
    };
}
