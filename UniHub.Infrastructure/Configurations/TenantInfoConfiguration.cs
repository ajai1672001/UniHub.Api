using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Domain.Entities;

namespace UniHub.Infrastructure.Configurations
{
    public class TenantInfoConfiguration : IEntityTypeConfiguration<TenantInfo>
    {
        public void Configure(EntityTypeBuilder<TenantInfo> builder)
        {
            builder.ToTable("TenantInfos", "tenant");


            builder.Property(x => x.ContactName).HasMaxLength(150);
            builder.Property(x => x.Email).HasMaxLength(200);
            builder.Property(x => x.PhoneNumber).HasMaxLength(20);
            builder.Property(x => x.AlternatePhoneNumber).HasMaxLength(20);

            builder.Property(x => x.AddressLine1).HasMaxLength(250);
            builder.Property(x => x.AddressLine2).HasMaxLength(250);
            builder.Property(x => x.City).HasMaxLength(100);
            builder.Property(x => x.State).HasMaxLength(100);
            builder.Property(x => x.Country).HasMaxLength(100);
            builder.Property(x => x.PostalCode).HasMaxLength(20);

            builder.Property(x => x.AboutUs).HasColumnType("nvarchar(max)");
            builder.Property(x => x.Vision).HasColumnType("nvarchar(max)");
            builder.Property(x => x.Mission).HasColumnType("nvarchar(max)");
            builder.Property(x => x.Description).HasColumnType("nvarchar(max)");

            builder.Property(x => x.LogoUrl).HasMaxLength(500);
            builder.Property(x => x.WebsiteUrl).HasMaxLength(300);

            // Only one TenantInfo per Tenant
            builder.HasIndex(x => x.TenantId).IsUnique();
        }
    }
}
