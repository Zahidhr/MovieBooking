using MovieBooking.Domain.Screenings;

namespace MovieBooking.Application.Interfaces.Persistence;

public interface IHoldRepository
{
    Task AddAsync(Hold hold, CancellationToken ct);
}
