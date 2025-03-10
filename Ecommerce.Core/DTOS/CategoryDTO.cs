using Ecommerce.Core.Entities;

namespace Ecommerce.Core.DTOS
{
    public class CategoryDTO
    {
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public int? DepartmentId { get; set; }
    }
}
