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
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.subTotal).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.DeliveryCost).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p=>p.Status).HasConversion<string>();


            builder.HasOne(o=>o.User).WithMany().HasForeignKey(d => d.UserId);
            builder.HasOne(o => o.Address).WithMany().HasForeignKey(d => d.AddressId);



            // builder.Property(p => p.PaymentIntentId).IsRequired().HasMaxLength(100);
            //builder.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(100);
            //builder.Property(p => p.CustomerEmail).IsRequired().HasMaxLength(100);
            //builder.Property(p => p.CustomerId).IsRequired().HasMaxLength(100);




        }
    }
}
