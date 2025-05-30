using System.Security.Cryptography;

namespace FinTech.Domain.ValueObjects;

public sealed record AccountNumber
{
    public string Value { get; }

    public AccountNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Account number cannot be empty", nameof(value));

        if (value.Length != 10 || !value.All(char.IsDigit))
            throw new ArgumentException("Account number must be 10 digits", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;

    public static AccountNumber GenerateRandom()
    {
        var randomNumber = RandomNumberGenerator.GetInt32(1, int.MaxValue)
            .ToString()
            .PadLeft(10, '0')[..10];
        return new AccountNumber(randomNumber);
    }
}
