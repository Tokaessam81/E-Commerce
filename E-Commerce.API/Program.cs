using AspNetCoreRateLimit;
using E_Commerce.API.Extentions;
using E_Commerce.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace E_Commerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddContext(builder.Configuration);  
            builder.Services.AddServices();
            
      
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);
            app.UseIpRateLimiting();
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var _DbContext = service.GetRequiredService<ECommerceDbContext>();

            var Log = service.GetRequiredService<ILoggerFactory>();

            try
            {
                await _DbContext.Database.MigrateAsync();

            }
            catch (Exception ex)
            {
                var logger = Log.CreateLogger<Program>();
                logger.LogError(ex, "Migration Error!");
            }
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
               app.UseSwaggerUI();         
            }
           

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
