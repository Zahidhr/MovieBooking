namespace MovieBooking.Application.Exceptions;

public sealed class NotFoundException : BusinessException
{
    public NotFoundException(string message, string errorCode = "NOT_FOUND")
        : base(message, errorCode)
    {
    }
}
