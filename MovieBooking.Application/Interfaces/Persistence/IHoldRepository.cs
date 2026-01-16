using MovieBooking.Domain.Screenings;

namespace MovieBooking.Application.Interfaces.Persistence;

public interface IHoldRepository
{
    /// <summary>
    /// Stages a new Hold entity for insertion. Does NOT call SaveChanges.
    /// </summary>
    void Add(Hold hold);
}
