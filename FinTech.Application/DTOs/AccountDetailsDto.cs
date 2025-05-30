using FinTech.Domain.Entities;

namespace FinTech.Application.DTOs;

public record AccountDetailsDto: AccountDto
{
    public IEnumerable<TransactionDto> Transactions { get; init; }= [];

    public new static AccountDetailsDto FromDomain(Account account) => new()
    {
        AccountNumber = account.Number.Value,
        OwnerName = account.OwnerName,
        Balance = account.Balance.Amount,
        Transactions = account.Transactions.Select(TransactionDto.FromDomain)
    };
}
