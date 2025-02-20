using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.Core.Specifications
{
    public class BaseSpecification<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Creteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDec { get; set; }
        public int Skip { get ; set; }
        public int Take { get ; set; }
        public bool IsPagination { get; set; } = false;

        public BaseSpecification(Expression<Func<T, bool>> creteria)
        {
            Creteria = creteria;
        }
        public BaseSpecification()
        {

        }
        public void AddOrderBy(Expression<Func<T, object>> OrderByexpression)
        {
            OrderBy=OrderByexpression;
        } 
        public void AddOrderByDesc(Expression<Func<T, object>> OrderByDescexpression)
        {
            OrderByDec = OrderByDescexpression;
        }
       public void AddPagination(int _Skip,int _Take)
       {
            IsPagination = true;
            Skip = _Skip;
            Take = _Take;
       }

    }
}
