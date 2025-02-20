using E_Commerce.Repository.Data;
using E_Commerce.Repository.Repositories;
using Ecommerce.Core;
using Ecommerce.Core.Entities;
using Ecommerce.Core.RepositoriesContract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository
{
    public class UnitOfWork : IUnitofWork
    {
        private readonly ECommerceDbContext _db;
        private readonly Hashtable _repo;

        public UnitOfWork(ECommerceDbContext db)
        {
            _db = db;
            _repo = new Hashtable();
        }

        public async Task<int> completeAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _db.DisposeAsync();
        }

        public IGenericRepo<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            //if Key is not available create object from TEntity
            var Key = typeof(TEntity).Name;
            if (!_repo.ContainsKey(Key))
            {
                var repository = new GenericRepo<TEntity>(_db);
                _repo.Add(Key, repository);
            }
            return  _repo[Key] as IGenericRepo<TEntity>;
        }
    }
}
