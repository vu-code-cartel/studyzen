using Newtonsoft.Json;

namespace StudyZen.Infrastructure.Services;

public sealed class FileService : IFileService
{
    public void WriteToJsonFile<T>(string filePath, T objectToWrite)
    {
        using var writer = new StreamWriter(filePath, false);
        var contentsToWrite = JsonConvert.SerializeObject(objectToWrite, Formatting.Indented);

        writer.Write(contentsToWrite);
        writer.Flush();
        writer.Close();
    }

    public T? ReadFromJsonFile<T>(string filePath)
    {
        using var reader = new StreamReader(filePath);

        var fileContents = reader.ReadToEnd();
        reader.Close();

        return JsonConvert.DeserializeObject<T>(fileContents);
    }
}