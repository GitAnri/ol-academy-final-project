using DAL.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Project.DAL.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly IndividualsDbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWork(IndividualsDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}
