using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Core.Enum;
using UniHub.Domain.Entities;
using static UniHub.Core.KnownString;

namespace UniHub.Infrastructure.Configurations
{
    public class EmailLogConfiguration : IEntityTypeConfiguration<EmailLog>
    {
        public void Configure(EntityTypeBuilder<EmailLog> builder)
        {
            builder.ToTable("EmailLogs", Schema.Email, t =>
            {
                t.HasCheckConstraint("CK_EmailLogs_Status",$"[Status] IN ({string.Join(", ", Enum.GetValues(typeof(EmailStatusEnum)).Cast<int>())})");
            });

            builder
               .Property(e => e.Content)
               .IsRequired();

            builder
                .Property(e => e.ErrorMessage)
                .IsRequired(false);

            builder
                .Property(e => e.Status)
                .IsRequired();
        }
    }
}
