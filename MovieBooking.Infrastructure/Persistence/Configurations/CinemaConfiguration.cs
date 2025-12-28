using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieBooking.Domain.Cinemas;

namespace MovieBooking.Infrastructure.Persistence.Configurations;

public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
{
    public void Configure(EntityTypeBuilder<Cinema> builder)
    {
        builder.ToTable("Cinemas");

        builder.HasKey(c => c.CinemaId);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.City)
            .HasMaxLength(100);

        builder.Property(c => c.CreatedAt)
            .IsRequired();
    }
}
