
using Core.Entities.Core.Entities;

namespace Core.Interfaces
{
   
        public interface IUnitOfWork
        {
            public IGenericRepository<TEntity> Repository<TEntity>();
            public Task<int> Complete();
        }
    
}
