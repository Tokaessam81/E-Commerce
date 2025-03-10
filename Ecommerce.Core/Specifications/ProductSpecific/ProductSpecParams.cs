using Ecommerce.Core.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Specifications.ProductSpecific
{
    public class ProductSpecParams
    {
        public SortOptions sort { get; set; }
        public int? CatgoryId { get; set; }
     

        private const int MaxPagesize=10;
        private int pageSize;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPagesize ? MaxPagesize : value; }
        }

        public int PageIndex { get; set; }
        public string? SearchByName { get; set; }
    }
}
