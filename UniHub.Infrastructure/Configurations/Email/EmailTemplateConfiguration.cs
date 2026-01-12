using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Domain.Entities;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastructure.Configurations
{
    public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<EmailTemplate> builder)
        {
            builder.ToTable("EmailTemplates", Schema.Email);
            
            builder
                .HasOne<Tenant>()
                .WithMany()
                .HasPrincipalKey(t => t.Id)
                .HasForeignKey(e => e.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(e => e.TenantId)
                .IsRequired(false);

            builder
                .Property(e => e.Subject)
                .IsRequired();

            builder
                .Property(e => e.Body)
                .IsRequired();

            builder
                .Property(e => e.Text)
                .IsRequired();

            builder
                .Property(e => e.DefaultEmail)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(e=>e.IsActive)
                .IsRequired();
        }
    }
}
