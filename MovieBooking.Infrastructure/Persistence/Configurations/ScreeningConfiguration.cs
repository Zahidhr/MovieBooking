using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieBooking.Domain.Screenings;

namespace MovieBooking.Infrastructure.Persistence.Configurations;

public sealed class ScreeningConfiguration : IEntityTypeConfiguration<Screening>
{
    public void Configure(EntityTypeBuilder<Screening> builder)
    {
        builder.ToTable("Screenings");

        builder.HasKey(s => s.ScreeningId);

        builder.Property(s => s.RoomId).IsRequired();
        builder.Property(s => s.MovieId).IsRequired();

        builder.Property(s => s.StartTime).IsRequired();
        builder.Property(s => s.EndTime).IsRequired();
        builder.Property(s => s.CreatedAt).IsRequired();

        builder.HasIndex(s => new { s.RoomId, s.StartTime });
    }
}
