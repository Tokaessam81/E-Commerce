using Ecommerce.Core.Entities;
using Ecommerce.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.RepositoriesContract
{
    public interface IGenericRepo<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int? id);
        Task<IQueryable<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllSpecification(ISpecifications<T> specifications);
        Task<T> GetByIdSpecification(ISpecifications<T> specifications);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
