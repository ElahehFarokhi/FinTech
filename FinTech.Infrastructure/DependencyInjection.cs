using FinTech.Application.Interfaces;
using FinTech.Infrsdtructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FinTech.Infrsdtructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
        services.AddSingleton<ITransactionRepository, InMemoryTransactionRepository>();

        return services;
    }
}
