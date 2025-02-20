using Ecommerce.Core.Entities;
using Ecommerce.Core.RepositoriesContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core
{
    public interface IUnitofWork
    {
        IGenericRepo<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        Task<int> completeAsync();
    }
}
