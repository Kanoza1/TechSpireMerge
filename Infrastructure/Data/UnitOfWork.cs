using Core.Entities.Core.Entities;
using Core.Interfaces;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
   
        public class UnitOfWork : IUnitOfWork
        {
            private readonly IDentityUserDbContext _context;
            private Hashtable _repositories;

            public UnitOfWork(IDentityUserDbContext context)
            {
                _context = context;
            }


            public async Task<int> Complete()
            => await _context.SaveChangesAsync();

            public IGenericRepository<TEntity> Repository<TEntity>() 
            {
                if (_repositories is null)
                    _repositories = new Hashtable();
                var type = typeof(TEntity).Name;
                if (!_repositories.ContainsKey(type))
                {

                    var repositoryType = typeof(GenericRepository<>);
                    var referenceInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                    _repositories.Add(type, referenceInstance);

                }
                return (IGenericRepository<TEntity>)_repositories[type];
            }
        }
    
}
