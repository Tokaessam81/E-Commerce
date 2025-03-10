using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.DepartmentSpecific
{
    public class DepartmentSpecification:BaseSpecification<Department>
    {
        public DepartmentSpecification(DepartmentSpecParams spec) : base(
        d =>
        (string.IsNullOrEmpty(spec.SearchByName) || d.EnglishName.ToLower().Contains(spec.SearchByName.ToLower())))
        {
            Includes.Add(D => D.Categories);


            if (spec.PageSize != 0 && spec.PageIndex != 0)
            {
                AddPagination((spec.PageIndex - 1) * spec.PageSize, spec.PageSize);
            }

        }
        public DepartmentSpecification(int Id) : base(D => D.Id == Id)
        {
            Includes.Add(D => D.Categories);
        }
    }
}
