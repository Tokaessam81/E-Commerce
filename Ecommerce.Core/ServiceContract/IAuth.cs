using Ecommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.ServiceContract
{
    public interface IAuth
    {
        Task<string> GenerateToken(AppUser user, UserManager<AppUser> userManager);
    }
}
