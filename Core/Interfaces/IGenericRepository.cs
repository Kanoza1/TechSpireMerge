using Core.Entities.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PersonalLearning.Specifications.ISpecification;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> 
    {
        public Task<List<T>> ListAsync(ISpecification<T> spec);
        public Task<T> GetEntityWithSpecification(ISpecification<T> spec);

        public Task<T> GetByIdAsync(string id);
        public Task<int> CountAsync(ISpecification<T> spec);
        public Task<IReadOnlyList<T>> GetAllAsync();
        public void Add(T entity);
        public Task AddRange(List<T> entities);

        public void Update(T entity);
        public void Delete(T entity);
    }
}
