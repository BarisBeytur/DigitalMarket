using DigitalMarket.Base.Entity;
using DigitalMarket.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DigitalMarket.Data.GenericRepository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{

    private readonly DigitalMarketDbContext _context;

    public GenericRepository(DigitalMarketDbContext context)
    {
        _context = context;
    }

    public async Task<List<TEntity>> GetAll(params string[] includes)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => EntityFrameworkQueryableExtensions.Include(current, include));
        }
        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetById(long Id, params string[] includes)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
        return await query.FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task<TEntity> Insert(TEntity entity)
    {
        entity.IsActive = true;
        entity.InsertDate = DateTime.UtcNow;
        entity.InsertUser = "System";
        await _context.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public async Task Delete(long Id)
    {
        var entity = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(_context.Set<TEntity>(), x => x.Id == Id);
        if (entity is not null)
            _context.Set<TEntity>().Remove(entity);
    }

    public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression, params string[] includes)
    {
        var query = _context.Set<TEntity>().AsQueryable();
        query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
        return query.Where(expression).FirstOrDefault();
    }

    public async Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> expression, params string[] includes)
    {
        var query = _context.Set<TEntity>().Where(expression).AsQueryable();
        query = includes.Aggregate(query, (current, inc) => EntityFrameworkQueryableExtensions.Include(current, inc));
        return await query.ToListAsync();
    }
}
