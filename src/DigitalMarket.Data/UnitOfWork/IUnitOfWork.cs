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
        Task Complete();
        Task CompleteWithTransaction();
        IGenericRepository<TEntity> Repository { get; }
    }
}
