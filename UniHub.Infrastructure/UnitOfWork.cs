using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace UniHub.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public void BeginTransaction()
    {
        if (_transaction == null)
        {
            _transaction = _context.Database.BeginTransaction();
        }
    }

    public async Task BeginTransactionAsync()
    {
        if (_transaction == null)
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
    }

    public int Commit()
    {
        var result = _context.SaveChanges();
        _transaction?.Commit();
        return result;
    }

    public async Task<int> CommitAsync()
    {
        var result = await _context.SaveChangesAsync();
        if (_transaction != null)
            await _transaction.CommitAsync();

        return result;
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}