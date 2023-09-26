namespace StudyZen.Infrastructure.Services;

public interface IFileService
{
    void WriteToJsonFile<T>(string filePath, T objectToWrite);
    T? ReadFromJsonFile<T>(string filePath);
}