using Ecommerce.Core.Common.Models;
using Ecommerce.Core.DTOS;
using Ecommerce.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.Core.ServiceContract
{
    public interface IUserService
    {
        Task<PaginatedResult<UserOperationsDTO>> GetUsersAsync(string? search, int page, int pageSize);
        Task RemoveUserAsync(string Id);
    }
}
