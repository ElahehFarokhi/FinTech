namespace FinTech.Domain.Exceptions;

public class InsufficientFundsException(string accountNumber, decimal currentBalance) : Exception($"Account {accountNumber} has insufficient funds. Current balance: {currentBalance:C}")
{
    public string AccountNumber { get; } = accountNumber;
    public decimal CurrentBalance { get; } = currentBalance;
}
