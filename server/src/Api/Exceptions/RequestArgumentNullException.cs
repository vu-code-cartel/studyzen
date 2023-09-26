namespace StudyZen.Api.Exceptions;

public sealed class RequestArgumentNullException : ArgumentNullException
{
    public RequestArgumentNullException(string? paramName = null, string? message = null)
        : base(paramName, message)
    {
    }
}