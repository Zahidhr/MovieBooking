using MovieBooking.Domain.Layouts;

namespace MovieBooking.Application.Interfaces.Persistence;

public interface ILayoutRepository
{
    Task AddAsync(Layout layout, CancellationToken ct);
    Task<Layout?> GetByIdAsync(Guid layoutId, CancellationToken ct);
    Task<bool> ExistsAsync(Guid layoutId, CancellationToken ct);

}
