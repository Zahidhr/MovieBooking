using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieBooking.Domain.Cinemas;

namespace MovieBooking.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");

        builder.HasKey(r => r.RoomId);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.HasOne(r => r.Cinema)
            .WithMany() // no Rooms collection in Cinema for now
            .HasForeignKey(r => r.CinemaId);

        builder.HasIndex(r => new { r.CinemaId, r.Name })
            .IsUnique();
    }
}
