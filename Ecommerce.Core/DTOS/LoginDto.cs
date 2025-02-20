using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Core.DTOS
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [DefaultValue("")]

        public string Email { get; set; }
        [DefaultValue("")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
      
    }
}
