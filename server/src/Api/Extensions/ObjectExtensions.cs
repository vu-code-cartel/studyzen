using StudyZen.Api.Exceptions;

namespace StudyZen.Api.Extensions;

public static class ObjectExtensions
{
    public static T ThrowIfRequestArgumentNull<T>(this T? obj, string? paramName = null) where T : class
    {
        if (obj is null)
        {
            throw new RequestArgumentNullException(
                paramName, paramName is null ? null : $"Request argument '{paramName}' is null.");
        }

        return obj;
    }
}