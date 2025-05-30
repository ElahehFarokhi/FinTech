namespace FinTech.Domain.Exceptions;

public class AccountNotFoundException(string accountNumber) : Exception($"Account {accountNumber} not found")
{
    public string AccountNumber { get; } = accountNumber;
}
