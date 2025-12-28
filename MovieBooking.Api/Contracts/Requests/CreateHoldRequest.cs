namespace MovieBooking.Api.Contracts.Requests;

public record SeatRequest(int RowNumber, int SeatNumber);

public record CreateHoldRequest(List<SeatRequest> Seats);
