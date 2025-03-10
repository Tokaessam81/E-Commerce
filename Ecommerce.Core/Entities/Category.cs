using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class Category:BaseEntity
    {
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
