namespace MovieBooking.Application.Interfaces.Services;

public interface IHoldCleanupService
{
    Task CleanupExpiredAsync(CancellationToken ct);
    Task CleanupExpiredByScreeningAsync(Guid screeningId, CancellationToken ct);
}
