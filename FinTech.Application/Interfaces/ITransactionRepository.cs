using FinTech.Application.DTOs;
using FinTech.Domain.Entities;
using FinTech.Domain.ValueObjects;

namespace FinTech.Application.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetByFiltersAsync(TransactionFilters filters);
}
