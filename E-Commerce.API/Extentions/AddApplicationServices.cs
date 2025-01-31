
using AspNetCoreRateLimit;
using E_Commerce.Repository.Data;
using E_Commerce.Services;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.ServiceContract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.API.Extentions
{
    public static class AddApplicationServices 
    {
       
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            services.AddIdentity<AppUser, UserRole>()
           .AddEntityFrameworkStores<ECommerceDbContext>()
          .AddDefaultTokenProviders();
            services.AddScoped<UserManager<AppUser>>();
            services.AddScoped<SignInManager<AppUser>>();

            services.AddScoped<IAuth, Auth>();
            services.AddControllers();
               services.AddSwaggerGen(c =>
               {
                   c.SwaggerDoc("v1", new() { Title = "E_Commerce.API", Version = "v1" });
                   c.AddSecurityDefinition("Bearer" +
                       "Bearer", new OpenApiSecurityScheme
                   {
                       Name = "Authentication",
                       BearerFormat = "JWT",
                       Scheme = "Bearer",
                       In = ParameterLocation.Header,
                       Type = SecuritySchemeType.Http,
                       Description = "Your Token"
                   });
               });
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var JwtSettings = builder.Build().GetSection("JwtSetting");
          var clientsecret = JwtSettings.GetSection("Authentication:Google:clientsecret");
          var ClientId = JwtSettings.GetSection("Authentication:Google:ClientId");

            services.AddAuthentication(option =>
               {
                   option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

               }).AddGoogle(googleOptions =>
               {
                   googleOptions.ClientId = ClientId.ToString();
                   googleOptions.ClientSecret = clientsecret.ToString();
               })
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = JwtSettings["ValidIssuer"],
                      ValidAudience = JwtSettings["ValidAudience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key))
                  };
                  
              });
           services.AddAuthorization();
            services.AddLocalization(services =>
            {
                services.ResourcesPath = "Resources";
            });
            var SupportedCultures = new []
            {
                new CultureInfo("en"),
                new CultureInfo("ar")
            };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en");
                options.SupportedCultures = SupportedCultures;
                options.SupportedUICultures = SupportedCultures;
            });
            services.AddLogging();
            

            #region AddRateLimitedServices
            services.AddMemoryCache();
            services.Configure<IpRateLimitOptions>(option =>
            {
                option.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Limit = 5,
                        Period = "1m"
                    }
                };

            });
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
            #endregion
            return services;

        }
        public static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ECommerceDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;

        }
       

    }

}
