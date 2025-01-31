using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.ServiceContract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace E_Commerce.Services
{
    public class Auth : IAuth
    {
        private readonly IConfiguration _configuration;

        public Auth(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string? GenerateToken()
        {
            var JwtSettings = _configuration.GetSection("JwtSetting");
           
            var tokenHandler = new JwtSecurityTokenHandler();
            var Key = Encoding.UTF8.GetBytes(JwtSettings["Key"]!);
            var AppUser = new AppUser();
            var Role = new UserRole();
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "Toka"),
                    new Claim(ClaimTypes.Role, "Admin"),
                    new Claim(ClaimTypes.Role, "User")

                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = JwtSettings["ValidIssuer"],
                Audience = JwtSettings["ValidAudience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(TokenDescriptor);
            return  tokenHandler.WriteToken(token);
        }


    }
}
