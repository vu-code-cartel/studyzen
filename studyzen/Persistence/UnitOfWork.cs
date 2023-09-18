using StudyZen.Courses;
using Studyzen.Lectures;

namespace StudyZen.Persistence;

public interface IUnitOfWork
{
    IGenericRepository<Course> Courses { get; }
    IGenericRepository<Lecture> Lectures { get; }
}

public sealed class UnitOfWork : IUnitOfWork
{
    public IGenericRepository<Course> Courses { get; }
    public IGenericRepository<Lecture> Lectures { get; }


    public UnitOfWork()
    {
        Courses = new GenericRepository<Course>("courses");
        Lectures = new GenericRepository<Lecture>("lectures");
    }
}
