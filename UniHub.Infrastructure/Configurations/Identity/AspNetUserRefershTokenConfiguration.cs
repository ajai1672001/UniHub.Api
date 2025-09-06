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
    public class AspNetUserRefershTokenConfiguration : IEntityTypeConfiguration<AspNetUserRefershToken>
    {
        public void Configure(EntityTypeBuilder<AspNetUserRefershToken> builder)
        {
            builder.ToTable("AspNetUserRefershTokens", Schema.Identity);
        }
    }
}
