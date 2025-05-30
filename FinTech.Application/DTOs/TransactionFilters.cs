
using FinTech.Domain.Enums;
using FinTech.Domain.ValueObjects;

namespace FinTech.Application.DTOs;

public record TransactionFilters
{
    public AccountNumber AccountNumber { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public TransactionType? Type { get; set; }
}