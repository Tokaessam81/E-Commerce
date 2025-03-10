using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.DepartmentSpecific
{
    public class DepartmentSpecPagination : BaseSpecification<Department>
    {
        public DepartmentSpecPagination(DepartmentSpecParams spec) : base(
        d =>
        (string.IsNullOrEmpty(spec.SearchByName) || d.EnglishName.ToLower().Contains(spec.SearchByName.ToLower())))
        {
            

        }
    

    }
}
