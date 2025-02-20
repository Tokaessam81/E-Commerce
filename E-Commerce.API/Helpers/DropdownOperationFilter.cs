using Ecommerce.Core.Entities;
using Ecommerce.Core;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.API.Helpers
{
    public class DropdownOperationFilter : IOperationFilter
    {
        private readonly IServiceProvider _serviceProvider;


        public DropdownOperationFilter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var categoryParameter = operation.Parameters.FirstOrDefault(p => p.Name == "CatgoryId");
            var DepartmentParameter = operation.Parameters.FirstOrDefault(p => p.Name == "DepartmentId");
           // var UserParameter = operation.Parameters.FirstOrDefault(p => p.Name == "search");
            if (categoryParameter != null)
            {
                using var scope = _serviceProvider.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitofWork>();

                var categories = unitOfWork.Repository<Category>().GetAllAsync().Result;

                categoryParameter.Schema.Enum = categories
                    .Select(c => new OpenApiString($"{c.Id}"))
                    .Cast<IOpenApiAny>()
                    .ToList();
            }
            if (DepartmentParameter != null)
            {
                using var scope = _serviceProvider.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitofWork>();

                var departments = unitOfWork.Repository<Department>().GetAllAsync().Result;

                DepartmentParameter.Schema.Enum = departments
                    .Select(c => new OpenApiString($"{c.Id}"))
                    .Cast<IOpenApiAny>()
                    .ToList();
            } 
          // if (UserParameter != null)
          // {
          //     using var scope = _serviceProvider.CreateScope(); 
          //     var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
          //     var users = userManager.Users.AsNoTracking();
          //
          //     UserParameter.Schema.Enum = users
          //         .Select(c => new OpenApiString($"{c.UserName}"))
          //         .Cast<IOpenApiAny>()
          //         .ToList();
          // }
        }
    }
}
