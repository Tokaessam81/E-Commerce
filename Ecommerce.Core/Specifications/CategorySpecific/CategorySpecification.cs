using Ecommerce.Core.Entities;
using Ecommerce.Core.Specifications.DepartmentSpecific;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.CategorySpecific
{
    public class CategorySpecification : BaseSpecification<Category>
    {
        public CategorySpecification(CategorySpecParams spec): 
            base(
            P => (!spec.DepartmentId.HasValue || P.DepartmentId == spec.DepartmentId)
            )
        {
            Includes.Add(D => D.Products);


            if (spec.PageSize != 0 && spec.PageIndex != 0)
            {
                AddPagination((spec.PageIndex - 1) * spec.PageSize, spec.PageSize);
            }

        }
        public CategorySpecification(int Id) : base(D => D.Id == Id)
        {
            Includes.Add(D => D.Products);
        }
    }
}
