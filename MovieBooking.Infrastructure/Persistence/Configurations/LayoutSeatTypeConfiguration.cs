using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieBooking.Domain.Layouts;

namespace MovieBooking.Infrastructure.Persistence.Configurations;

public class LayoutSeatTypeConfiguration : IEntityTypeConfiguration<LayoutSeatType>
{
    public void Configure(EntityTypeBuilder<LayoutSeatType> builder)
    {
        builder.ToTable("LayoutSeatTypes");

        builder.HasKey(x => new { x.LayoutId, x.RowNumber, x.SeatNumber });

        builder.Property(x => x.RowNumber).IsRequired();
        builder.Property(x => x.SeatNumber).IsRequired();

        // SeatType enum -> tinyint
        builder.Property(x => x.SeatType)
            .IsRequired()
            .HasConversion<byte>();
    }
}
