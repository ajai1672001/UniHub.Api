using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Domain.Entities.Identity;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastructure.Configurations
{
    public class UserOtpConfiguration : IEntityTypeConfiguration<UserOtp>
    {
        void IEntityTypeConfiguration<UserOtp>.Configure(EntityTypeBuilder<UserOtp> builder)
        {
            builder.ToTable("UserOtps", Schema.Identity);
        }
    }
}
