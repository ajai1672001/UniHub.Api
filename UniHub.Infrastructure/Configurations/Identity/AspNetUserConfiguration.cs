using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Core.Enum;
using UniHub.Domain.Entities.Identity;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastructure.Configurations
{
    public class AspNetUserConfiguration : IEntityTypeConfiguration<AspNetUser>
    {
        public void Configure(EntityTypeBuilder<AspNetUser> builder)
        {
            builder.ToTable("AspNetUsers", Schema.Identity); 
            
            builder
                .Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(e => e.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(e => e.Gender)
                .HasDefaultValue(GenderEnum.NotSpecified)
                .IsRequired();

            builder
                .Property(e => e.DateOfBirth)
                .IsRequired();
        }
    }
}
