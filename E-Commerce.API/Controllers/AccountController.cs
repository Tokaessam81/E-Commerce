using E_Commerce.API.DTOS;
using E_Commerce.API.Error;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.ServiceContract;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;

namespace E_Commerce.API.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuth _Auth;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,IAuth auth )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _Auth = auth;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]   

        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            //Check if mail is in the database
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));
            //Check if password is correct
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            //Generate Token
            var token = _Auth.GenerateToken();
            return Ok(new UserDto()
            {
                Email = user.Email,
                Token = token
            });
        }
        [HttpPost("Register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            var user = new AppUser
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PasswordHash = registerDto.Password
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));
            var token = _Auth.GenerateToken();
            return Ok(new UserDto()
            {
                Email = user.Email,
                Token = token
            });
        }


        [HttpPost("Login/google")]
        public async Task<ActionResult<UserDto>> GoogleLogin([FromBody] GoogleAuthRequestDTO request)
        {
            var client = new HttpClient();
            var googleTokenInfoEndpoint = $"https://oauth2.googleapis.com/tokeninfo?id_token={request.IdToken}";

            var response = await client.GetAsync(googleTokenInfoEndpoint);
            if (!response.IsSuccessStatusCode)
            {
                return Unauthorized(new { message = "Invalid Google token." });
            }

            var payload = await response.Content.ReadAsStringAsync();
            var tokenInfo = JsonConvert.DeserializeObject<GoogleTokenInfoDTO>(payload);

            // تحقق من التوكن وأضف المستخدم إلى النظام
            // قم بإنشاء JWT خاص بك وإرساله للمستخدم
            var jwtToken = _Auth.GenerateToken();

            return Ok(new UserDto { Email=tokenInfo.Email,Token=jwtToken});
        }

    }
}
