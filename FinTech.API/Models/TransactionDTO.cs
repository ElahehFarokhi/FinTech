using System.ComponentModel.DataAnnotations;

namespace FinTech.API.Models;

public record TransactionAmountRequest
{
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }
}

public record TransferRequest()
{
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }
    public required string TargetAccountNumber { get; set; }
}

public record TransactionSearchParameter(string? Type, DateTime? FromDate, DateTime? ToDate);

