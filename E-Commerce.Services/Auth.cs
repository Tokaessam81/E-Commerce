using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.ServiceContract;
using Microsoft.AspNetCore.Identity;
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
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> GenerateToken(AppUser user, UserManager<AppUser> userManager)
        {
            // التحقق من كون المستخدم غير null
            if (user == null) throw new ArgumentNullException(nameof(user));

            var privateClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email , user.Email),
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                privateClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtKey = _configuration["JwtSetting:Key"]
                ?? throw new ArgumentNullException("JwtSetting:Key is missing in configuration!");

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var audience = _configuration["JwtSetting:ValidAudience"]
                ?? throw new ArgumentNullException("JwtSetting:ValidAudience is missing in configuration!");

            var issuer = _configuration["JwtSetting:ValidIssuer"]
                ?? throw new ArgumentNullException("JwtSetting:ValidIssuer is missing in configuration!");

            var expirationMinutes = double.TryParse(_configuration["JwtSetting:AccessTokenExpiration"], out var expiration)
                                    ? expiration : 30;
            var tokenExpiration = TimeSpan.FromMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                audience: audience,
                issuer: issuer,
                expires: DateTime.UtcNow.Add(tokenExpiration),
                claims: privateClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
      

    }
}
