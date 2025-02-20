using Ecommerce.Core.Common.Enums;
using Ecommerce.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.Core.Specifications.ProductSpecific
{
    public class ProductSpecification:BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecParams spec) : base(
            P =>
            (string.IsNullOrEmpty(spec.SearchByName) || P.Name.ToLower().Contains(spec.SearchByName.ToLower())) &&
            (!spec.CatgoryId.HasValue || P.CategoryId == spec.CatgoryId))

        {
            Includes.Add(P => P.Category);
            Includes.Add(P => P.Images);
            if (!string.IsNullOrEmpty(spec.sort.ToString()))
            {
                if (Enum.TryParse<SortOptions>(spec.sort.ToString(), true, out var sortOption))
                {
                    switch (sortOption)
                    {
                        case SortOptions.PriceAsc:
                            AddOrderBy(P => P.Price);
                            break;
                        case SortOptions.PriceDesc:
                            AddOrderByDesc(P => P.Price);
                            break;
                        case SortOptions.RatingAsc:
                            AddOrderBy(P => P.Rating);
                            break;
                        case SortOptions.RatingDesc:
                            AddOrderByDesc(P => P.Rating);
                            break;
                        case SortOptions.DiscountAsc:
                            AddOrderBy(P => P.Discount);
                            break;
                        case SortOptions.DiscountDesc:
                            AddOrderByDesc(P => P.Discount);
                            break;
                        case SortOptions.NameAsc:
                            AddOrderBy(P => P.Name);
                            break;
                        case SortOptions.NameDesc:
                            AddOrderByDesc(P => P.Name);
                            break;
                        default:
                            AddOrderBy(P => P.Quantity);
                            break;
                    }
                }
                else
                    AddOrderBy(P => P.Id);

                if (spec.PageSize != 0 && spec.PageIndex != 0)
                {
                    AddPagination((spec.PageIndex - 1) * spec.PageSize, spec.PageSize);
                }


            }
        }
        public ProductSpecification(int Id):base(P=>P.Id==Id)
        {
            Includes.Add(P => P.Category);
            Includes.Add(P => P.Images);
        }

    }
}
