using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.DTOS
    {
        public class BasketDto
        {
            [Required(ErrorMessage = "BasketId is required")]
            public string BasketId { get; set; }

            [Required(ErrorMessage = "ProductId is required")]
            public string ProductId { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
            public int Quantity { get; set; }
        }
    }

