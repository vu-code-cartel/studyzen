using StudyZen.Application.Dtos;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public sealed class QuizService : IQuizService
{
    private readonly IUnitOfWork _unitOfWork;

    public QuizService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<QuizDto> CreateQuiz(CreateQuizDto dto)
    {
        // TODO: add validation

        var quiz = new Quiz(dto.Title);

        _unitOfWork.Quizzes.Add(quiz);
        await _unitOfWork.SaveChanges();

        return new QuizDto(quiz);
    }

    public async Task<QuizDto> GetQuiz(int quizId)
    {
        var quiz = await _unitOfWork.Quizzes.GetByIdChecked(quizId);
        return new QuizDto(quiz);
    }

    public async Task<IReadOnlyCollection<QuizDto>> GetAllQuizzes()
    {
        var quizzes = await _unitOfWork.Quizzes.Get();
        return quizzes.Select(q => new QuizDto(q)).ToList();
    }

    public async Task UpdateQuiz(int quizId, UpdateQuizDto dto)
    {
        // TODO: add validation

        var quiz = await _unitOfWork.Quizzes.GetByIdChecked(quizId);

        quiz.Title = dto.Title ?? quiz.Title;

        await _unitOfWork.SaveChanges();
    }

    public async Task DeleteQuiz(int quizId)
    {
        await _unitOfWork.Quizzes.DeleteByIdChecked(quizId);
        await _unitOfWork.SaveChanges();
    }

    public async Task<QuizQuestionDto> AddQuestionToQuiz(int quizId, CreateQuizQuestionDto dto)
    {
        // TODO: add validation

        var quiz = await _unitOfWork.Quizzes.GetByIdChecked(quizId);

        // Starting a new transaction to avoid circular reference between question and correct answer when saving
        await using var transaction = _unitOfWork.BeginTransaction();

        try
        {
            var question = new QuizQuestion(quizId, dto.Question, TimeSpan.FromSeconds(dto.TimeLimitInSeconds));

            var correctAnswer = new QuizAnswer(quizId, dto.CorrectAnswer);
            question.PossibleAnswers.Add(correctAnswer);

            foreach (var incorrectAnswer in dto.IncorrectAnswers)
            {
                var answer = new QuizAnswer(quizId, incorrectAnswer);
                question.PossibleAnswers.Add(answer);
            }

            quiz.Questions.Add(question);
            await _unitOfWork.SaveChanges();

            question.CorrectAnswer = correctAnswer;
            await _unitOfWork.SaveChanges();

            await transaction.CommitAsync();

            return new QuizQuestionDto(question);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<IReadOnlyCollection<QuizQuestionDto>> GetQuizQuestions(int quizId)
    {
        var quizQuestions = await _unitOfWork.QuizQuestions
            .Get(i => i.QuizId == quizId, includes: q => q.PossibleAnswers);
        return quizQuestions.Select(i => new QuizQuestionDto(i)).ToList();
    }
}
