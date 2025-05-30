using FinTech.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace FinTech.Application.DTOs;

public record AccountDto
{
    public string AccountNumber { get; init; }
    
    public string OwnerName { get; init; }
    
    public decimal Balance { get; init; }

    public static AccountDto FromDomain(Account account) => new()
    {
        AccountNumber = account.Number.Value,
        OwnerName = account.OwnerName,
        Balance = account.Balance.Amount
    };
}
