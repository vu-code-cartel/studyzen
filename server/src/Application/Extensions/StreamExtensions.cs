using System.Collections.Concurrent;

namespace StudyZen.Application.Extensions;

public static class StreamExtensions
{
    public static IReadOnlyCollection<string> ReadLines(this Stream stream)
    {
        var lines = new BlockingCollection<string>();
        using var reader = new StreamReader(stream);

        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            lines.Add(line);
        }

        lines.CompleteAdding();

        return lines;
    }
}