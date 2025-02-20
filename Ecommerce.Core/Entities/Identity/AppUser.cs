using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Entities.Identity
{
    public class AppUser:IdentityUser<int>
    {
      
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Lang= "En";  
        public DateTime JoiningDate { get; set; } = DateTime.Now;
        public ICollection<Order> Orders { get; set; }= new HashSet<Order>();
        public List<Address>? Addresses { get; set; }
      
    }
}
