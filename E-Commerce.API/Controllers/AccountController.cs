using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Core.ServiceContract;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.DTOS;
using E_Commerce.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Core.Common.Constants;
using System.Security.Claims;
using Bogus.Extensions.Extras;
using System.Linq;
using System.Net;
using System.Text.Json;
using E_Commerce.Services;
using E_Commerce.API.Error;
public class AccountController : BaseController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountController> _logger;
    private readonly IAuth _auth;
    private readonly IResponseCachedService _responseCachedService;

    public AccountController(UserManager<AppUser> userManager, IEmailService emailService,
        ILogger<AccountController> logger, IAuth auth,IResponseCachedService responseCachedService )
    {
        _userManager = userManager;
        _emailService = emailService;
        _logger = logger;
        _auth = auth;
        _responseCachedService = responseCachedService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status401Unauthorized)]

    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {

        var user = await _userManager.FindByEmailAsync(loginDto.Email).ConfigureAwait(false);
        if (user == null)
        {
            return Unauthorized(new ApiResponse(401, "Invalid email or password."));
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password).ConfigureAwait(false);
        if (!isPasswordValid)
        {
           // _logger.LogWarning("Login failed: Invalid password for {Email}", loginDto.Email);
            return Unauthorized(new ApiResponse(401, "Invalid email or password." ));
        }

        if (!await _userManager.IsEmailConfirmedAsync(user).ConfigureAwait(false))
        {
            return Unauthorized(new ApiResponse(401, "Email not confirmed. Please check your email." ));
        }

        var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

        var jwtToken = await _auth.GenerateToken(user, _userManager);

        return Ok(new UserDto
        {
            Email = user.Email,
            Role = roles.FirstOrDefault(),
            Token = jwtToken
        });
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            return BadRequest(new { Message = "This email is already registered." });
        }

        var tokenBytes = Guid.NewGuid().ToByteArray();
        var token = WebEncoders.Base64UrlEncode(tokenBytes);
        var cacheKey = $"pending_user_{model.Email}";
        var nameParts = model.UserName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        string firstName = nameParts.Length > 0 ? nameParts[0] : null!;
        string lastName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : null!;
        var cacheObject = new { Token = token, Password = model.Password ,FirstName= firstName, LastName= lastName };
        var cacheData = JsonSerializer.Serialize(cacheObject, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await _responseCachedService.CacheResponseAsync(cacheKey, cacheData, TimeSpan.FromHours(1));

        _logger.LogInformation($"Generated Token: {token} for {model.Email}");

        var confirmationLink = $"{Request.Scheme}://{Request.Host}/api/account/ConfirmEmail?email={model.Email}&token={token}";
        Console.WriteLine($"Confirmation Link: {confirmationLink}");

        await _emailService.SendEmailAsync(model.Email, "Confirm your email",
            $"Click <a href='{confirmationLink}'>here</a> to confirm your email.");

        return Ok(new { Message = "Please check your email to confirm your account." });
    }
    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string email, string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            return BadRequest(new { Message = "Invalid email or token." });

        try
        {
            token = WebUtility.UrlDecode(token);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error decoding token: {ex.Message}");
            return BadRequest(new { Message = "Invalid token format." });
        }

        var cacheKey = $"pending_user_{email}";
        var cachedData = await _responseCachedService.GetCachedResponseAsync(cacheKey);

        if (string.IsNullOrEmpty(cachedData))
        {
            return BadRequest(new { Message = "Invalid or expired confirmation token." });
        }

        _logger.LogInformation($"Raw data from Redis: {cachedData}");

        cachedData = cachedData.Trim('"');

        cachedData = cachedData.Replace("\\u0022", "\"");

        _logger.LogInformation($"Decoded Data: {cachedData}");

        try
        {
            var cacheObject = JsonSerializer.Deserialize<Dictionary<string, string>>(cachedData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (cacheObject == null || !cacheObject.ContainsKey("token") || !cacheObject.ContainsKey("password"))
            {
                _logger.LogError($"Cache data structure is invalid: {cachedData}");
                return BadRequest(new { Message = "Invalid token format." });
            }

            var storedToken = cacheObject["token"];
            var password = cacheObject["password"];
            var FirstName = cacheObject["firstName"];
            var LastName = cacheObject["lastName"];
            var username = FirstName+LastName;
       
            
            if (storedToken != token)
            {
                return BadRequest(new { Message = "Invalid or expired confirmation token." });
            }

            var newUser = new AppUser
            {
                UserName = username,
                Email = email,
                FirstName = FirstName,
                LastName = LastName,
                
                EmailConfirmed = true
            };

            var role = await _userManager.AddToRoleAsync(newUser,AuthorizationConstants.CustomerRole);
            var result = await _userManager.CreateAsync(newUser, password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _responseCachedService.RemoveAsync(cacheKey);
            var tokenResult = await _auth.GenerateToken(newUser, _userManager);

            return Ok(new { Message = "Email confirmed successfully. You can log in now.", YourToken = tokenResult });
        }
        catch (JsonException ex)
        {
            _logger.LogError($"Error parsing cached data: {ex.Message}");
            return BadRequest(new { Message = "Invalid cached data format." });
        }
    }
    [HttpPost("Login/google")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserDto>> GoogleLogin([FromBody] GoogleAuthRequestDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.IdToken))
        {
            return BadRequest(new { Message = "Google ID Token is required." });
        }

        var googleTokenInfoEndpoint = $"https://oauth2.googleapis.com/tokeninfo?id_token={request.IdToken}";
        using var client = new HttpClient();
        var response = await client.GetAsync(googleTokenInfoEndpoint);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Invalid Google token for login.");
            return Unauthorized(new { Message = "Invalid Google token." });
        }

        var payload = await response.Content.ReadAsStringAsync();
        var tokenInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenInfoDTO>(payload);

        if (tokenInfo == null || string.IsNullOrWhiteSpace(tokenInfo.Email))
        {
            _logger.LogWarning("Failed to retrieve user info from Google token.");
            return Unauthorized(new { Message = "Invalid Google token or missing email." });
        }

        var user = await _userManager.FindByEmailAsync(tokenInfo.Email);
        if (user == null)
        {
            user = new AppUser
            {
                UserName = tokenInfo.Email,
                Email = tokenInfo.Email,
                EmailConfirmed = true 
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                _logger.LogError("Failed to create user for Google login: {Errors}", string.Join(", ", createResult.Errors));
                return BadRequest(new { Message = "Failed to create user.", Errors = createResult.Errors.Select(e => e.Description) });
            }

            await _userManager.AddToRoleAsync(user, AuthorizationConstants.CustomerRole);
            
        }
      var Role=  await _userManager.GetRolesAsync(user);
        var jwtToken = await _auth.GenerateToken(user, _userManager);

        _logger.LogInformation("User logged in successfully using Google: {Email}", user.Email);

        // إرجاع بيانات المستخدم مع التوكن
        return Ok(new UserDto
        {
            Email = user.Email,
            Role = Role.FirstOrDefault(),
            Token = jwtToken
        });
    }


}