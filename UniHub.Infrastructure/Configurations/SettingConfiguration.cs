using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniHub.Domain.Entities;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastructure.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable("Settings", Schema.Default);

            builder
                .Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired()
                .IsUnicode(true);

            builder
                .Property(e => e.Content)
                .IsRequired();
        }
    }
}