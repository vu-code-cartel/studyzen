using Newtonsoft.Json;
using StudyZen.Common.Exceptions;

namespace StudyZen.Common;

public static class Utilities
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

    public static void WriteToJsonFile<T>(string filePath, T objectToWrite)
    {
        using var writer = new StreamWriter(filePath, false);
        var contentsToWrite = JsonConvert.SerializeObject(objectToWrite, Formatting.Indented);

        writer.Write(contentsToWrite);
        writer.Flush();
        writer.Close();
    }

    public static T? ReadFromJsonFile<T>(string filePath)
    {
        using var reader = new StreamReader(filePath);

        var fileContents = reader.ReadToEnd();
        reader.Close();

        return JsonConvert.DeserializeObject<T>(fileContents);
    }
}