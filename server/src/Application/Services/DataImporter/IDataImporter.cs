namespace StudyZen.Application.Services;

public interface IDataImporter
{
    IList<TResult> Import<T1, T2, TResult>(
        IReadOnlyCollection<string> lines,
        Func<T1, T2, TResult> instanceCreator)
        where T1 : notnull
        where T2 : notnull;
}