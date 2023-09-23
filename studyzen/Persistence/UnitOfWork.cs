using StudyZen.Courses;
using Studyzen.Lectures;
using StudyZen.Flashcards;
using StudyZen.FlashcardSetClass;

namespace StudyZen.Persistence;

public interface IUnitOfWork
{
    IGenericRepository<Course> Courses { get; }
    IGenericRepository<Lecture> Lectures { get; }
    IGenericRepository<Flashcard> Flashcards { get; }
    IGenericRepository<FlashcardSet> FlashcardSets { get; }
}

public sealed class UnitOfWork : IUnitOfWork
{
    public IGenericRepository<Course> Courses { get; }
    public IGenericRepository<Lecture> Lectures { get; }
    public IGenericRepository<Flashcard> Flashcards { get; }
    public IGenericRepository<FlashcardSet> FlashcardSets { get; }

    public UnitOfWork()
    {
        Courses = new GenericRepository<Course>("courses");
        Lectures = new GenericRepository<Lecture>("lectures");
        Flashcards = new GenericRepository<Flashcard>("flashcards");
        FlashcardSets = new GenericRepository<FlashcardSet>("flashcardSets");
    }
}
