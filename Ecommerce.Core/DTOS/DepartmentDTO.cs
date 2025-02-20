using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Ecommerce.Core.DTOS
{
    public class DepartmentDTO
    {
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string? PictureUrl { get; set; }
        public string? PictureDiscreption { get; set; }
       
    }
}
