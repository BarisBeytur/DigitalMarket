using DigitalMarket.Base.Entity;
using DigitalMarket.Data.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Data.UnitOfWork
{
    public interface IUnitOfWork<TEntity> where TEntity : BaseEntity
    {
        Task Commit();
        Task CommitWithTransaction();
        IGenericRepository<TEntity> Repository { get; }
        void Dispose();
    }
}
