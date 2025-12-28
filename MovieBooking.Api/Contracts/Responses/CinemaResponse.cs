namespace MovieBooking.Api.Contracts.Responses;

public record CinemaResponse(Guid CinemaId, string Name, string? City);
