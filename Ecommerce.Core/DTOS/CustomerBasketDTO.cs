
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTO
{
    public class CustomerBasketDTO
    {
        public string Id { get; set; } = string.Empty;
        public List<BasketItemDTO> Items { get; set; } = new();
    }
}
