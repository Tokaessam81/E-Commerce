using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public enum ProductStatus
    {
        [EnumMember(Value = "Available")]
        Available,
        [EnumMember(Value = "NotAvailable")]
        NotAvailable,
    
    }
}
