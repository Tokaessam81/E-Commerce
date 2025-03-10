using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Data.Configrations
{
    public class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.ArabicName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.EnglishName).IsRequired().HasMaxLength(100);

            builder.HasOne(c => c.Department)
      .WithMany(d => d.Categories)
      .HasForeignKey(c => c.DepartmentId)
      .OnDelete(DeleteBehavior.SetNull);




        }
    }
}
