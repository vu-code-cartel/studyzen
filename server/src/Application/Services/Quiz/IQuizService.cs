using StudyZen.Application.Dtos;

namespace StudyZen.Application.Services;

public interface IQuizService
{
    Task<QuizDto> CreateQuiz(CreateQuizDto dto);
    Task<QuizDto> GetQuiz(int quizId);
    Task<IReadOnlyCollection<QuizDto>> GetAllQuizzes();
    Task UpdateQuiz(int quizId, UpdateQuizDto dto);
    Task DeleteQuiz(int quizId);
    Task<QuizQuestionDto> AddQuestionToQuiz(int quizId, CreateQuizQuestionDto dto);
    Task<IReadOnlyCollection<QuizQuestionDto>> GetQuizQuestions(int quizId);
}
