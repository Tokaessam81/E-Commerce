using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.Core.Specifications.ProductSpecific
{
    public class ProductSpecPagination:BaseSpecification<Product>
    {
        public ProductSpecPagination(ProductSpecParams spec) : base(
            P =>
            (string.IsNullOrEmpty(spec.SearchByName) || P.Name.ToLower().Contains(spec.SearchByName.ToLower())) &&
            (!spec.CatgoryId.HasValue || P.CategoryId == spec.CatgoryId)
            )
        {
                
        }

    }
}
