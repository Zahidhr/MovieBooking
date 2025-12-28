using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieBooking.Domain.Layouts;

namespace MovieBooking.Infrastructure.Persistence.Configurations;

public class LayoutConfiguration : IEntityTypeConfiguration<Layout>
{
    public void Configure(EntityTypeBuilder<Layout> builder)
    {
        builder.ToTable("Layouts");

        builder.HasKey(l => l.LayoutId);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.RowCount).IsRequired();
        builder.Property(l => l.SeatsPerRow).IsRequired();
        builder.Property(l => l.CreatedAt).IsRequired();

        builder.HasMany(l => l.SeatTypes)
            .WithOne(st => st.Layout!)
            .HasForeignKey(st => st.LayoutId);
    }
}
