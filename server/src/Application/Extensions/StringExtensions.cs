namespace StudyZen.Application.Extensions;

public static class StringExtensions
{
    public static T Convert<T>(this string value) where T : notnull
    {
        return (T)System.Convert.ChangeType(value, typeof(T));
    }
}