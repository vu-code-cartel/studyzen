using Microsoft.EntityFrameworkCore.Storage;

namespace StudyZen.Application.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICourseRepository Courses { get; }
    ILectureRepository Lectures { get; }
    IFlashcardSetRepository FlashcardSets { get; }
    IFlashcardRepository Flashcards { get; }
    IQuizRepository Quizzes { get; }
    IQuizQuestionRepository QuizQuestions { get; }
    IQuizAnswerRepository QuizAnswers { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    Task<int> SaveChanges();
    IDbContextTransaction BeginTransaction();
}