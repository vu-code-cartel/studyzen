namespace StudyZen.Application.Exceptions;

public class IdentifiableException : Exception
{
    public string ErrorCode { get; }

    public IdentifiableException(string errorCode)
    {
        ErrorCode = errorCode;
    }
}
