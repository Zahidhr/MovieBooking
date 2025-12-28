namespace MovieBooking.Api.Contracts.Responses;

public record RoomResponse(Guid RoomId, Guid CinemaId, Guid LayoutId, string Name);
