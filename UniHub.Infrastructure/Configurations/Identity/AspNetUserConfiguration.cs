using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniHub.Core.Enum;
using UniHub.Entities;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastruture.Configurations
{
    public class AspNetUserConfiguration : IEntityTypeConfiguration<AspNetUser>
    {
        public void Configure(EntityTypeBuilder<AspNetUser> builder)
        {
            builder.ToTable("AspNetUsers", Schema.Identity);

            builder
                .Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(x => x.DateOfBirth)
                .IsRequired();

            builder
                .Property(x => x.Gender)
                .HasDefaultValue(GenderEnum.NotSpecified)
                .IsRequired();

            builder
                .Property(x => x.IsDeleted)
                .IsRequired();

            builder
                .HasQueryFilter(e => !e.IsDeleted);
        }
    }
}