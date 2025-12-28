using Microsoft.EntityFrameworkCore;
using MovieBooking.Domain.Cinemas;
using MovieBooking.Domain.Layouts;
using MovieBooking.Domain.Movies;
using MovieBooking.Domain.Screenings;

namespace MovieBooking.Infrastructure.Persistence;

public class MovieBookingDbContext : DbContext
{
    public MovieBookingDbContext(DbContextOptions<MovieBookingDbContext> options)
        : base(options) { }

    public DbSet<Cinema> Cinemas => Set<Cinema>();
    public DbSet<Layout> Layouts => Set<Layout>();
    public DbSet<LayoutSeatType> LayoutSeatTypes => Set<LayoutSeatType>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Screening> Screenings => Set<Screening>();
    public DbSet<Hold> Holds => Set<Hold>();
    public DbSet<SeatReservation> SeatReservations => Set<SeatReservation>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovieBookingDbContext).Assembly);
    }
}
