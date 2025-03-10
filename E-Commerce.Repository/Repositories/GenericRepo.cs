using E_Commerce.Repository.Data;
using Ecommerce.Core.Entities;
using Ecommerce.Core.RepositoriesContract;
using Ecommerce.Core.Specifications;
using Ecommerce.Repository.Specifications.Repo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
    {
        private readonly ECommerceDbContext _db;

        public GenericRepo(ECommerceDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(T entity) 
        {
            await _db.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity) 
        {
             _db.Set<T>().Remove(entity);
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
           return await Task.Run(() => _db.Set<T>());
        }

        public async Task<IReadOnlyList<T>> GetAllSpecification(ISpecifications<T> specifications)
        {
            return await ApplyQuery(specifications).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int? id) 
        {

           return await  _db.Set<T>().FindAsync(id) ;
        }

        public async Task<T> GetByIdSpecification(ISpecifications<T> specifications)
        {
            return await ApplyQuery(specifications).FirstOrDefaultAsync();
        }

        public void Update(T entity)
        {
            _db.Set<T>().Update(entity);    
        }
        private IQueryable<T> ApplyQuery(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_db.Set<T>(), spec);
        }
    }
}
