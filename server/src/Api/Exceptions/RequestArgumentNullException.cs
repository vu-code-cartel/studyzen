using System.Diagnostics.CodeAnalysis;

namespace StudyZen.Api.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class RequestArgumentNullException : ArgumentNullException
{
    public RequestArgumentNullException(string? paramName = null, string? message = null)
        : base(paramName, message)
    {
    }
}