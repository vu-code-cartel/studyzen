using StudyZen.Application.Extensions;
using System.Collections.Concurrent;

namespace StudyZen.Application.Services;

public abstract class DataImporter : IDataImporter
{
    public IList<TResult> Import<T1, T2, TResult>(
        IReadOnlyCollection<string> lines,
        Func<T1, T2, TResult> instanceCreator)
        where T1 : notnull
        where T2 : notnull
    {
        var importedInstances = new BlockingCollection<TResult>();
        var threads = new List<Thread>();
        Exception? localException = null;

        var batches = lines.Split(3);

        foreach (var batch in batches)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    ProcessLines(batch, instanceCreator, ref importedInstances);
                }
                catch (Exception ex)
                {
                    localException = ex;
                }
            });

            thread.Start();
            threads.Add(thread);
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        importedInstances.CompleteAdding();

        // rethrow exception on main thread to avoid process termination
        if (localException is not null)
        {
            throw localException;
        }

        return importedInstances.GetConsumingEnumerable().ToList();
    }

    protected abstract void ProcessLines<T1, T2, TResult>(
        IEnumerable<string> lines,
        Func<T1, T2, TResult> instanceCreator,
        ref BlockingCollection<TResult> importedInstances)
        where T1 : notnull
        where T2 : notnull;
}