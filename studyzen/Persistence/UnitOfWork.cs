using StudyZen.Courses;
using Studyzen.Lectures;
using StudyZen.FlashCards;
using StudyZen.FlashCardSetClass;

namespace StudyZen.Persistence;

public interface IUnitOfWork
{
    IGenericRepository<Course> Courses { get; }
    IGenericRepository<Lecture> Lectures { get; }
    IGenericRepository<FlashCard> FlashCards { get; }
    IGenericRepository<FlashCardSet> FlashCardSets { get; }
}

public sealed class UnitOfWork : IUnitOfWork
{
    public IGenericRepository<Course> Courses { get; }
    public IGenericRepository<Lecture> Lectures { get; }
    public IGenericRepository<FlashCard> FlashCards { get; }
    public IGenericRepository<FlashCardSet> FlashCardSets { get; }

    public UnitOfWork()
    {
        Courses = new GenericRepository<Course>("courses");
        Lectures = new GenericRepository<Lecture>("lectures");
        FlashCards = new GenericRepository<FlashCard>("flashCards");
        FlashCardSets = new GenericRepository<FlashCardSet>("flashCardSets");
    }
}
