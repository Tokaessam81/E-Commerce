using Ecommerce.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.RepositoriesContract
{
    public interface IUserRepository
    {
        Task<AppUser?> GetUserByEmailAsync(string email);
    }

}
