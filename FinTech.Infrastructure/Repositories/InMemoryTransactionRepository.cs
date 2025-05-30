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

    public Task<IEnumerable<Transaction>> GetByFiltersAsync(AccountNumber accountNumber, string? type = null, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _transactions
            .Where(t => t.SourceAccount == accountNumber || t.DestinationAccount == accountNumber);
            
        if (type != null)
            query = query.Where(t => (int)t.Type == Int32.Parse(type)).ToList();
        if (fromDate != null)
            query = query.Where(t => t.Timestamp >= fromDate).ToList();
        if (toDate != null)
            query = query.Where(t => t.Timestamp <= toDate).ToList();

        query = query.AsEnumerable();
        return Task.FromResult(query);
    }
}
