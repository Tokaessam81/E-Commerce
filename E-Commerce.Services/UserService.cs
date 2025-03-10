using Ecommerce.Core.Common.Models;
using Ecommerce.Core.DTOS;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.ServiceContract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<PaginatedResult<UserOperationsDTO>> GetUsersAsync(string? search, int page, int pageSize)
        {
            var query = _userManager.Users.AsNoTracking();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
            }
            var totalUsers = await query.CountAsync();

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            for (int i = 0; i < users.Count; i++)
            {
                users[i].Id = i+1;
            }
            var userDtos = users.Select(user => new UserOperationsDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedDate = user.JoiningDate
            }).ToList();

            return new PaginatedResult<UserOperationsDTO>(userDtos, totalUsers, page, pageSize);
        }

        public async Task RemoveUserAsync(string Id)
        {
            var User= await _userManager.FindByIdAsync(Id);
            if (User != null)
            await _userManager.DeleteAsync(User);


        }
    }
}

