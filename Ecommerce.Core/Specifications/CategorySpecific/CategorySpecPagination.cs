using Ecommerce.Core.Entities;
using Ecommerce.Core.Specifications.CategorySpecific;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.Core.Specifications.CategorySpecific
{
    public class CategorySpecPagination:BaseSpecification<Category>
    {
        public CategorySpecPagination(CategorySpecParams spec) : base(
            P =>
            (!spec.DepartmentId.HasValue  || P.DepartmentId == spec.DepartmentId)
            )
        {
                
        }

    }
}
