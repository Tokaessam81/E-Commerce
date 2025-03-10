using Ecommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
            builder.Property(p => p.SubTotal).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.DeliveryCost).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p=>p.Status).HasConversion<string>();


            builder.HasOne(o => o.User)
             .WithMany(u=>u.Orders)
             .HasForeignKey(d => d.UserId)
             .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.ShippingAddress)
                .WithMany()
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.SetNull);

               
           



        }
    }
}
