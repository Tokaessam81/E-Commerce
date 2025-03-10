using AspNetCoreRateLimit;
using E_Commerce.API.Error;
using E_Commerce.API.Helpers;
using E_Commerce.Repository.Data;
using E_Commerce.Repository.Repositories;
using E_Commerce.Repository;
using E_Commerce.Services;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.RepositoriesContract;
using Ecommerce.Core.ServiceContract;
using Ecommerce.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Ecommerce.Core.Services.Contract;
using Ecommerce.Core.Repository.Contract;
using Ecommerce.Services;
using Ecommerce.Core.Common.Constants;
using Ecommerce.Core.Entities;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using E_Commerce.API.Filters.EduCredit.Service.Filters;

namespace E_Commerce.API.Extentions
{
    public static class AddApplicationServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            #region Add Services
            services.AddControllers()
     ;


            // Identity Configuration
            services.AddIdentity<AppUser, UserRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<ECommerceDbContext>()
            .AddDefaultTokenProviders();
            services.AddScoped<RoleManager<UserRole>>();
            services.AddScoped<UserManager<AppUser>>();
            services.AddScoped<SignInManager<AppUser>>();

            // Dependency Injection

            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            services.AddScoped<IUnitofWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IPaymentServices, PaymentService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IBasketService), typeof(BasketService));
            services.AddScoped<IResponseCachedService, ResponseCashedService>();
            services.AddScoped<IAuth, Auth>();
            services.AddHttpClient();
            services.AddAutoMapper(typeof(MappingProfile));
             
            #endregion
            #region Swagger Configuration
            services.AddSwaggerGen(c =>
            {
                var serviceProvider = services.BuildServiceProvider();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "E-Commerce API", Version = "v1" });
                c.OperationFilter<DropdownOperationFilter>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Please enter a valid JWT token"
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();

            });
            #endregion

            #region Authentication & Authorization
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var jwtSettings = new JwtSettings();
            builder.Build().GetSection("JwtSetting").Bind(jwtSettings);

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudience = jwtSettings.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                };

                
            })
            
            ;

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConstants.AdminPolicy, policy => policy.RequireRole(AuthorizationConstants.AdminRole));
                options.AddPolicy(AuthorizationConstants.CustomerPolicy, policy => policy.RequireRole(AuthorizationConstants.CustomerRole));
            });
          
            #endregion

            #region CORS Configuration
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            #endregion

            #region Logging
            services.AddLogging();
            #endregion

            #region Rate Limiting
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            services.Configure<IpRateLimitOptions>(options =>
            {
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Limit = 100,
                        Period = "1m"
                    }
                };
            });

            #endregion

            #region Validation Error Services
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                                               .Where(m => m.Value.Errors.Any())
                                               .SelectMany(m => m.Value.Errors)
                                               .Select(e => e.ErrorMessage)
                                               .ToList();

                    var response = new ApiResponseValidation
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });
            #endregion

            return services;
        }

        #region Connection String
        public static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ECommerceDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
               
            });

            return services;
        }

        #endregion
    }
}
