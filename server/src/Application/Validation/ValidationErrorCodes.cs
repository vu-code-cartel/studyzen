namespace StudyZen.Application.Validation;

public static class ValidationErrorCodes
{
    public const string Null = "Null";
    public const string NullOrWhitespace = "NullOrWhitespace";
    public const string MaxLength = "MaxLength";
    public const string NotFound = "NotFound";
    public const string TooSmall = "TooSmall";
    public const string TooLarge = "TooLarge";
    public const string InvalidFileFormat = "InvalidFileFormat";
    public const string IncorrectArgumentCount = "IncorrectArgumentCount";
}