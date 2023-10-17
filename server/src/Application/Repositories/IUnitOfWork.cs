namespace StudyZen.Application.Repositories;

public interface IUnitOfWork
{
    ICourseRepository Courses { get; }
    ILectureRepository Lectures { get; }
    IFlashcardSetRepository FlashcardSets { get; }
    IFlashcardRepository Flashcards { get; }
    Task SaveChanges();
}