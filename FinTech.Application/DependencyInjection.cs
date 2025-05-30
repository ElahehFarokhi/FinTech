
using FinTech.Application.Interfaces;
using FinTech.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinTech.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();

        return services;
    }
}
