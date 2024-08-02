using DigitalMarket.Base.Entity;
using System.Linq.Expressions;

namespace DigitalMarket.Data.GenericRepository;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<List<TEntity>> GetAll(params string[] includes);
    Task<TEntity?> GetById(long Id, params string[] includes);
    Task<TEntity> Insert(TEntity entity);
    Task InsertRange(IEnumerable<TEntity> entities);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task Delete(long Id);
    Task<List<TEntity>> Where(Expression<Func<TEntity, bool>> expression, params string[] includes);
    Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> expression, params string[] includes);
}
