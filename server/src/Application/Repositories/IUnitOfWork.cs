namespace StudyZen.Application.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICourseRepository Courses { get; }
    ILectureRepository Lectures { get; }
    IFlashcardSetRepository FlashcardSets { get; }
    IFlashcardRepository Flashcards { get; }
    Task<int> SaveChanges();
}