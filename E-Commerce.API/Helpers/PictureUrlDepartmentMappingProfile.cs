using AutoMapper;
using Ecommerce.Core.DTOS;
using Ecommerce.Core.Entities;

namespace E_Commerce.API.Helpers
{
    public class PictureUrlDepartmentMappingProfile : IValueResolver<Department, DepartmentDTO ,string>
    {
        private readonly IConfiguration _config;

        public PictureUrlDepartmentMappingProfile(IConfiguration config)
        {
            _config = config;
        }
        public string Resolve(Department source, DepartmentDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{source.PictureUrl}";
            }
            return "";
        }
    } 
}
