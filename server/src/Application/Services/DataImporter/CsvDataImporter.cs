using StudyZen.Application.Extensions;
using System.Collections.Concurrent;
using StudyZen.Application.Exceptions;

namespace StudyZen.Application.Services;

public sealed class CsvDataImporter : DataImporter
{
    protected override void ProcessLines<T1, T2, TResult>(
        IEnumerable<string> lines,
        Func<T1, T2, TResult> instanceCreator,
        ref BlockingCollection<TResult> importedInstances)
    {
        foreach (var line in lines)
        {
            var values = line.Split(',');
            if (values.Length != 2)
            {
                throw new IncorrectArgumentCountException("FileContent");
            }

            var instance = instanceCreator.Invoke(
                values[0].Convert<T1>(),
                values[1].Convert<T2>());

            importedInstances.Add(instance);
        }
    }
}