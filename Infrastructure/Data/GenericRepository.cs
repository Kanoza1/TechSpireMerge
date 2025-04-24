using Core.Entities.Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PersonalLearning.Specifications.ISpecification;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDentityUserDbContext context;

        public GenericRepository(IDentityUserDbContext context)
        {
            this.context = context;
            
        }
        public void Add(T entity)
       => context.Set<T>().Add(entity);
        public async Task AddRange(List<T> entities)
        { 
            await context.Set<T>().AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }
        public void Delete(T entity)
               => context.Set<T>().Remove(entity);

        public async Task<T> GetEntityWithSpecification(ISpecification<T> spec)
        => await ApplySpecification(spec).FirstOrDefaultAsync();
        public async Task<List<T>> ListAsync(ISpecification<T> spec)
    => await ApplySpecification(spec).ToListAsync();



        public IQueryable<T> ApplySpecification(ISpecification<T> spec)
        => SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), spec);
        public async Task<IReadOnlyList<T>> GetAllAsync()
       => await context.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(string id)
        => await context.Set<T>().FindAsync(id);

        public void Update(T entity)
              => context.Set<T>().Update(entity);

        public async Task<int> CountAsync(ISpecification<T> spec)
              => await ApplySpecification(spec).CountAsync();

    }
}
