using Studyzen.Common.Errors;

namespace Studyzen.Common;

public static class Utilities
{
    public static T ThrowIfRequestArgumentNull<T>(this T? obj, string? paramName = null) where T : class
    {
        if (obj is null)
        {
            throw new RequestArgumentNullException(
                paramName,
                paramName is null
                    ? null
                    : $"Request argument '{paramName}' is null.");
        }

        return obj;
    }
}