using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieBooking.Domain.Screenings;

namespace MovieBooking.Infrastructure.Persistence.Configurations;

public sealed class SeatReservationConfiguration : IEntityTypeConfiguration<SeatReservation>
{
    public void Configure(EntityTypeBuilder<SeatReservation> builder)
    {
        builder.ToTable("SeatReservations");

        builder.HasKey(s => new { s.ScreeningId, s.RowNumber, s.SeatNumber });

        builder.Property(s => s.Status).IsRequired().HasConversion<byte>();
        builder.Property(s => s.CreatedAt).IsRequired();

        builder.HasIndex(s => new { s.ScreeningId, s.Status });
        builder.HasIndex(s => s.ExpiresAt);
    }
}
