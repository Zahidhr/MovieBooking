namespace MovieBooking.Api.Contracts.Responses;

public record LayoutSeatTypeResponse(
    int RowNumber,
    int SeatNumber,
    string SeatType
);

public record LayoutResponse(
    Guid LayoutId,
    string Name,
    int RowCount,
    int SeatsPerRow,
    List<LayoutSeatTypeResponse> SeatTypes
);
