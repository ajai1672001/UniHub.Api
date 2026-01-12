using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Domain.Entities;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastructure.Configurations;

public class EmailRecieverConfiguration : IEntityTypeConfiguration<EmailReciever>
{
    public void Configure(EntityTypeBuilder<EmailReciever> builder)
    {
        builder.ToTable("EmailReciever", Schema.Email);

        builder
            .Property(e => e.Email)
            .IsRequired();

        builder
            .HasOne(e=>e.EmailLog)
            .WithMany(el=>el.EmailRecivers)
            .HasForeignKey(e=>e.EmailLogId)
            .HasPrincipalKey(e=>e.Id)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
