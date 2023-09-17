using StudyZen.Courses;
using StudyZen.FlashCards;

namespace StudyZen.Persistence;

public interface IUnitOfWork
{
    IGenericRepository<Course> Courses { get; }
    IGenericRepository<FlashCard> FlashCards { get; }
}

public sealed class UnitOfWork : IUnitOfWork
{
    public IGenericRepository<Course> Courses { get; }
    public IGenericRepository<FlashCard> FlashCards { get; }

    public UnitOfWork()
    {
        Courses = new GenericRepository<Course>("courses");
        FlashCards = new GenericRepository<FlashCard>("flashCards");
    }
}
