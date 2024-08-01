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

    /// <summary>
    /// Returns the repository for the entity
    /// </summary>
    public IGenericRepository<TEntity> Repository => new GenericRepository<TEntity>(_context);


    /// <summary>
    /// Completes the unit of work
    /// </summary>
    /// <returns></returns>
    public async Task Commit()
    {
        await _context.SaveChangesAsync();
    }


    /// <summary>
    /// Completes the unit of work with a transaction
    /// </summary>
    /// <returns></returns>
    public async Task CommitWithTransaction()
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


    /// <summary>
    /// Disposes the context
    /// </summary>
    public void Dispose()
    {
        if (_context != null)
        {
            _context.Dispose();
        }
    }
}   
