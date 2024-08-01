using DigitalMarket.Base.Entity;
using DigitalMarket.Data.Context;
using DigitalMarket.Data.GenericRepository;
using System.Reflection.Metadata.Ecma335;

namespace DigitalMarket.Data.UnitOfWork;

public class UnitOfWork<TEntity> : IDisposable, IUnitOfWork<TEntity> where TEntity : BaseEntity
{
    private readonly DigitalMarketDbContext _context;

    public UnitOfWork(DigitalMarketDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<TEntity> Repository => new GenericRepository<TEntity>(_context);

    public async Task Complete()
    {
        await _context.SaveChangesAsync();
    }

    public async Task CompleteWithTransaction()
    {
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
            }
        }
    }

    public void Dispose()
    {
        if (_context != null)
        {
            _context.Dispose();
        }
    }
}   
