using System.Diagnostics.CodeAnalysis;

namespace StudyZen.Application.Exceptions;

[ExcludeFromCodeCoverage]
public sealed class AccessDeniedException : Exception
{
    public AccessDeniedException()
        : base("User has no rights to modify the content")
    {
    }
}