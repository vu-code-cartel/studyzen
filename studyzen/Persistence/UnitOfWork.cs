using StudyZen.Courses;

namespace StudyZen.Persistence;

public interface IUnitOfWork
{
    IGenericRepository<Course> Courses { get; }
}

public sealed class UnitOfWork : IUnitOfWork
{
    public IGenericRepository<Course> Courses { get; }

    public UnitOfWork()
    {
        Courses = new GenericRepository<Course>("courses");
    }
}
