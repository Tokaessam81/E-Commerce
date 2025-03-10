using Ecommerce.Core.Entities;
using Ecommerce.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Data.Configrations
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasOne(a => a.AppUser)
    .WithMany(u => u.Addresses)
    .HasForeignKey(a => a.AppUserId)
    .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
