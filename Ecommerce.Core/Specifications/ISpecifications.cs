using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.Core.Specifications
{
    public interface ISpecifications<T> where T : class
    {
        public Expression<Func<T,bool>> Creteria { get; set; }
        public List<Expression<Func<T,object>>>Includes { get; set; }
        public Expression<Func<T,object>> OrderBy { get; set; }
        public Expression<Func<T,object>> OrderByDec { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagination { get; set; }


    }
}
