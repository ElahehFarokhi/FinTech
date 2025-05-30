using FinTech.Application.Interfaces;
using FinTech.Domain.Entities;
using FinTech.Domain.ValueObjects;

namespace FinTech.API.SeedData;

public static class SeedData
{
    public static async Task InitializeAsync(IAccountRepository accountRepo)
    {
        if (!await accountRepo.ExistsAsync(new AccountNumber("1000000000")))
        {
            await accountRepo.AddAsync(new Account(
                new AccountNumber("1000000000"),
                new Money(5000),
                "be1b"));
            await accountRepo.AddAsync(new Account(
                new AccountNumber("2000000000"),
                new Money(10000),
                "Elaheh"));
            await accountRepo.AddAsync(new Account(
                new AccountNumber("3000000000"),
                new Money(30000),
                "Ryan"));
        }
    }
}
