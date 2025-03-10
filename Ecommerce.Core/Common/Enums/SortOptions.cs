using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Core.Common.Enums
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SortOptions
    {
        NameAsc,
        NameDesc,
        RatingDesc,
        RatingAsc,
        PriceDesc,
        PriceAsc,
        DiscountDesc,
        DiscountAsc
    }
}
