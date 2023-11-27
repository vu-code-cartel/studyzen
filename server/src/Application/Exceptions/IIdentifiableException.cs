using System.Diagnostics.CodeAnalysis;

namespace StudyZen.Application.Exceptions;

[ExcludeFromCodeCoverage]
public class IdentifiableException : Exception
{
    public string ErrorCode { get; }

    public IdentifiableException(string errorCode)
    {
        ErrorCode = errorCode;
    }
}
