namespace MovieBooking.Api.Contracts.Requests;

public record CreateRoomRequest(string Name, Guid LayoutId);
