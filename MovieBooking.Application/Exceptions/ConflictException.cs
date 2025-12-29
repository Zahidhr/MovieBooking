namespace MovieBooking.Application.Exceptions;

public sealed class ConflictException : BusinessException
{
    public ConflictException(string message, string errorCode = "CONFLICT")
        : base(message, errorCode) { }
}
