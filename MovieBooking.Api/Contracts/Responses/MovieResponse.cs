namespace MovieBooking.Api.Contracts.Responses;

public record MovieResponse(Guid MovieId, string Title, int DurationMinutes);
