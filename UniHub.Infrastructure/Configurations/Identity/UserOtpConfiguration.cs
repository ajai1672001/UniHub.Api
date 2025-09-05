using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UniHub.Infrastruture.Configurations
{
    public class UserOtpConfiguration : IEntityTypeConfiguration<UserOtp>
    {
        public void Configure(EntityTypeBuilder<UserOtp> builder)
        {
            GeneralConfiguration.ConfigBaseSoftDeleteIdAuditEntity<UserOtp,Guid>(builder,nameof(UserOtp));

            builder
                .Property(e => e.Otp)
                .HasMaxLength(6)
                .IsRequired();

            builder
                .Property(e=>e.IsUsed)
                .HasDefaultValue(false)
                .IsRequired();

            builder
                .HasOne<AspNetUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(e => e.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
