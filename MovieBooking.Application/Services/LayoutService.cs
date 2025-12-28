using MovieBooking.Application.Interfaces.Persistence;
using MovieBooking.Domain.Enums;
using MovieBooking.Domain.Layouts;

namespace MovieBooking.Application.Services;

public class LayoutService
{
    private readonly ILayoutRepository _layoutRepo;

    public LayoutService(ILayoutRepository layoutRepo)
    {
        _layoutRepo = layoutRepo;
    }

    public async Task<Guid> CreateAsync(
        string name,
        int rowCount,
        int seatsPerRow,
        IReadOnlyCollection<int>? vipRows,
        IReadOnlyCollection<int>? premiumRows,
        CancellationToken ct)
    {
        // minimal validation (no framework yet)
        if (rowCount <= 0) throw new ArgumentException("RowCount must be > 0");
        if (seatsPerRow <= 0) throw new ArgumentException("SeatsPerRow must be > 0");

        var vip = new HashSet<int>(vipRows ?? Array.Empty<int>());
        var premium = new HashSet<int>(premiumRows ?? Array.Empty<int>());

        // VIP and Premium rows should not overlap
        if (vip.Overlaps(premium))
            throw new ArgumentException("VIP rows and Premium rows cannot overlap.");

        var layout = new Layout(name, rowCount, seatsPerRow);

        // generate explicit seat-type mapping for every seat
        for (var row = 1; row <= rowCount; row++)
        {
            var seatType =
                vip.Contains(row) ? SeatType.Vip :
                premium.Contains(row) ? SeatType.Premium :
                SeatType.Standard;

            for (var seat = 1; seat <= seatsPerRow; seat++)
            {
                layout.SeatTypes.Add(new LayoutSeatType(layout.LayoutId, row, seat, seatType));
            }
        }

        await _layoutRepo.AddAsync(layout, ct);
        return layout.LayoutId;
    }

    public Task<Layout?> GetByIdAsync(Guid layoutId, CancellationToken ct)
        => _layoutRepo.GetByIdAsync(layoutId, ct);
}
