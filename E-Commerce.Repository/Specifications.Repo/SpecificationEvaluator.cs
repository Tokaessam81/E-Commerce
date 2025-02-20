
using Ecommerce.Core.Entities;
using Ecommerce.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.Repository.Specifications.Repo
{
    internal static class SpecificationEvaluator<TEntity>where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> innerQuery,ISpecifications<TEntity> Spec)
        {
            var query = innerQuery;
            if (Spec.Creteria is not null)
            {
                query=query.Where(Spec.Creteria);
            }
            if (Spec.OrderBy is not null)
            {
                query = query.OrderBy(Spec.OrderBy);
            }
            else if (Spec.OrderByDec is not null) 
            {
                query=query.OrderByDescending(Spec.OrderByDec);  
            }
            if (Spec.IsPagination)
            {
                query=query.Skip(Spec.Skip).Take(Spec.Take);
            }
            query=Spec.Includes.Aggregate(query,(currentQuery,IncludeExpressions)=>currentQuery.Include(IncludeExpressions));
            return query;
        }

    }
}
