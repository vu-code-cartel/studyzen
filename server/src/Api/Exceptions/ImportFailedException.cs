namespace StudyZen.Api.Exceptions;

public sealed class ImportFailedException : Exception
{
    public ImportFailedException(string message)
        : base(message)
    {
    }
}

