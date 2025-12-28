using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieBooking.Domain.Screenings;

namespace MovieBooking.Infrastructure.Persistence.Configurations;

public sealed class HoldConfiguration : IEntityTypeConfiguration<Hold>
{
    public void Configure(EntityTypeBuilder<Hold> builder)
    {
        builder.ToTable("Holds");

        builder.HasKey(h => h.HoldId);

        builder.Property(h => h.ScreeningId).IsRequired();
        builder.Property(h => h.Status).IsRequired().HasConversion<byte>();
        builder.Property(h => h.ExpiresAt).IsRequired();
        builder.Property(h => h.CreatedAt).IsRequired();

        builder.HasIndex(h => new { h.ScreeningId, h.Status });
        builder.HasIndex(h => h.ExpiresAt);
    }
}
