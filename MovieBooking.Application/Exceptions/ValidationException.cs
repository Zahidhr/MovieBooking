namespace MovieBooking.Application.Exceptions;

public sealed class ValidationException : BusinessException
{
    public ValidationException(string message, string errorCode = "VALIDATION_ERROR")
        : base(message, errorCode) { }
}
