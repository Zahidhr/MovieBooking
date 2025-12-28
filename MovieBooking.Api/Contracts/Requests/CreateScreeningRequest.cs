namespace MovieBooking.Api.Contracts.Requests;

public record CreateScreeningRequest(Guid MovieId, DateTimeOffset StartTime);
