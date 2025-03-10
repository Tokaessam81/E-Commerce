using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Data
{
    public class ECommerceContextSeedData
    {

        public async static Task SeedingAsync(ECommerceDbContext _dbContext)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory; // المسار الحالي للتطبيق
            var departmentPath = Path.Combine(basePath, "Departments.json");

            var DepartmentData = File.ReadAllText(departmentPath);
            var departments = JsonSerializer.Deserialize<List<Department>>(DepartmentData);
            if (departments?.Count > 0)
            {

                if (_dbContext.Departments.Count() == 0)
                {
                    foreach (var brand in departments)
                    {
                        await _dbContext.Departments.AddAsync(brand);

                    }
                    await _dbContext.SaveChangesAsync();
                }
            }

            var categoriesPath = Path.Combine(basePath, "Categories.json");

            var CategoriesData = File.ReadAllText(categoriesPath);
            var categories = JsonSerializer.Deserialize<List<Category>>(CategoriesData);
            if (categories?.Count > 0)
            {

                if (_dbContext.Categories.Count() == 0)
                {
                    foreach (var category in categories)
                    {
                        await _dbContext.Categories.AddAsync(category);

                    }
                    await _dbContext.SaveChangesAsync();
                }
            }


            if (await _dbContext.Products.AnyAsync()) return; 

            try
            {
                var productPath = Path.Combine(basePath, "Products.json");
                var ProductData = File.ReadAllText(productPath);
                var products = JsonSerializer.Deserialize<List<Product>>(ProductData);

                if (products == null) return;

                var productEntities = products.Select(p => new Product
                {
                    Name = p.Name,
                    Price = p.Price,
                    Discount = p.Discount,
                    Quantity = p.Quantity,
                    Color = p.Color,
                    Rating = p.Rating,
                    status = p.status,
                    CategoryId = p.CategoryId,
                    Images = p.Images.Select(url => new ProductImage { ImageUrl = url.ImageUrl}).ToList()
                }).ToList();

                _dbContext.Products.AddRange(productEntities);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration Error: {ex.Message}");
            }
            var deliveryPath = Path.Combine(basePath, "delivery.json");

            var DeliveryData = File.ReadAllText(deliveryPath);
            var deliverymethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);
            if (deliverymethods?.Count > 0)
            {

                if (_dbContext.DeliveryMethods.Count() == 0)
                {
                    foreach (var method in deliverymethods)
                    {
                        _dbContext.Set<DeliveryMethod>().Add(method);

                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
