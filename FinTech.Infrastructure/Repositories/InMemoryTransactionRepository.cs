using FinTech.Application.DTOs;
using FinTech.Application.Interfaces;
using FinTech.Domain.Entities;
using FinTech.Domain.ValueObjects;
using System;

namespace FinTech.Infrsdtructure.Repositories;

public class InMemoryTransactionRepository : ITransactionRepository
{
    private readonly List<Transaction> _transactions = [];

    public Task AddAsync(Transaction transaction)
    {
        _transactions.Add(transaction);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Transaction>> GetByFiltersAsync(TransactionFilters filters)
    {
        var query = _transactions
            .Where(t => t.SourceAccount == filters.AccountNumber || t.DestinationAccount == filters.AccountNumber);
            
        if (filters.Type.HasValue)
            query = query.Where(t => t.Type == filters.Type.Value).ToList();
        if (filters.FromDate.HasValue)
            query = query.Where(t => t.Timestamp >= filters.FromDate.Value).ToList();
        if (filters.ToDate.HasValue)
            query = query.Where(t => t.Timestamp <= filters.ToDate.Value).ToList();

        query = query.AsEnumerable();
        return Task.FromResult(query);
    }
}
