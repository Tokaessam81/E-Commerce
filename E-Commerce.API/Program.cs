using AspNetCoreRateLimit;
using E_Commerce.API.Error;
using E_Commerce.API.Extentions;
using E_Commerce.Repository.Data;
using E_Commerce.Repository.Identity;
using Ecommerce.Core.Common.Constants;
using Ecommerce.Core.Entities.Identity;
using Ecommerce.Core.ServiceContract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddContext(builder.Configuration);
            builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSetting"));
            builder.Services.AddServices();
            var RedisConnection = builder.Configuration.GetConnectionString("Redis");
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(RedisConnection));



            // Configure the HTTP request pipeline.
            var app = builder.Build();
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            await RoleSeeder.SeedRolesAndAdminsAsync(service);
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();
            var userManager = service.GetRequiredService<UserManager<AppUser>>();
            var emailService = service.GetRequiredService<IEmailService>();
            var _DbContext = service.GetRequiredService<ECommerceDbContext>();
            var Log = service.GetRequiredService<ILoggerFactory>();

            try
            {
                await _DbContext.Database.MigrateAsync();
                await ECommerceContextSeedData.SeedingAsync(_DbContext);
            }
            catch (Exception ex)
            {
                var logger = Log.CreateLogger<Program>();
                logger.LogError(ex, "Migration Error!");
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 401)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync(
                        System.Text.Json.JsonSerializer.Serialize(new ApiResponse(401, "Unauthorized!"))
                    );
                }
            });
            app.UseIpRateLimiting();
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 401)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    await context.HttpContext.Response.WriteAsync(
                        System.Text.Json.JsonSerializer.Serialize(new ApiResponse(401,"Unauthorized access. Please provide a valid token."))
                    );
                }
            });
            app.UseStatusCodePagesWithRedirects("errors/{0}");

                app.UseSwagger();
            app.UseSwaggerUI();

            

            app.UseCors();

            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization(); 

            app.MapControllers();

            app.Run();

        }
    }
}
