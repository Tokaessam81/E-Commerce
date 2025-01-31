using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities
{
    public class Department:BaseEntity
    {
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string? PictureUrl { get; set; }
        public string? PictureDiscreption { get; set; }
        public ICollection<Category> Categories { get; set; }= new HashSet<Category>();
    }
}
