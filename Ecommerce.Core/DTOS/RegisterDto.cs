using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [DefaultValue("")]
    [Required(ErrorMessage = "UserName is required.")]
    [StringLength(50, ErrorMessage = "Must be between 3 and 50 characters.", MinimumLength = 3)]
    public string UserName { get; set; }
   
    [DefaultValue("")]

    [Required(ErrorMessage = "Email is required.")]
    [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]{2,}$",
        ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }
    [DefaultValue("")]

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, contain one uppercase letter, one lowercase letter, one number, and one special character.")]
    public string Password { get; set; }
    [DefaultValue("")]
    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Confirm Password must match Password.")]
    public string ConfirmPassword { get; set; }
}
