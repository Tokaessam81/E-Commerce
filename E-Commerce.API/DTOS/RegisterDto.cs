
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.API.DTOS
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "UserName is required")]
        [StringLength(50, ErrorMessage = "Must be between 3 and 50 characters", MinimumLength = 3)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "Must be a valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "\"Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character (@$!%*?&).\"\r\n")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Confirm Password Must match Password")]
        public string ConfirmPassword { get; set; }

    }
}
