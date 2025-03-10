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
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
          builder.ToTable("Departments");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.ArabicName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.EnglishName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.PictureDiscreption).IsRequired(false);
            builder.Property(p => p.PictureUrl).IsRequired(false);
           
        }
    }
}
