namespace MovieBooking.Api.Contracts.Requests;

public record CreateLayoutRequest(
    string Name,
    int RowCount,
    int SeatsPerRow,
    List<int>? VipRows,
    List<int>? PremiumRows
);
