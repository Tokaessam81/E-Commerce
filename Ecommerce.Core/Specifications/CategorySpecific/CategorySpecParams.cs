using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.CategorySpecific
{
    public class CategorySpecParams
    {
        public int? DepartmentId { get; set; }
        private const int MaxPagesize = 10;
        private int pageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPagesize ? MaxPagesize : value; }
        }

        public int PageIndex { get; set; }
    }
}
