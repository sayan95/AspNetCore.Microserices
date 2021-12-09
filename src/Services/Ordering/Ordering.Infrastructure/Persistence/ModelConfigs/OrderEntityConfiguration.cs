using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence.ModelConfigs
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(o => o.Id).IsUnique();
            builder.Property(o => o.Username).IsRequired().HasMaxLength(50);
            builder.Property(o => o.EmailAddress).IsRequired();
            builder.Property(o => o.TotalPrice).IsRequired();
        }
    }
}
