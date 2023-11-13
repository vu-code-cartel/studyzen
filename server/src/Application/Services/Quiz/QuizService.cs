using StudyZen.Application.Dtos;
using StudyZen.Application.Extensions;
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

        return quiz.ToDto();
    }

    public async Task<QuizDto> GetQuiz(int quizId)
    {
        var quiz = await _unitOfWork.Quizzes.GetByIdChecked(quizId);
        return quiz.ToDto();
    }

    public async Task<IReadOnlyCollection<QuizDto>> GetAllQuizzes()
    {
        var quizzes = await _unitOfWork.Quizzes.Get();
        return quizzes.ToDto();
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

        var question = new QuizQuestion(quizId, dto.Question, TimeSpan.FromSeconds(dto.TimeLimitInSeconds));

        foreach (var incorrectAnswer in dto.IncorrectAnswers)
        {
            var answer = new QuizAnswer(quizId, incorrectAnswer, false);
            question.Choices.Add(answer);
        }

        foreach (var correctAnswer in dto.CorrectAnswers)
        {
            var answer = new QuizAnswer(quizId, correctAnswer, true);
            question.Choices.Add(answer);
        }

        quiz.Questions.Add(question);
        await _unitOfWork.SaveChanges();

        return question.ToDto();
    }

    public async Task<IReadOnlyCollection<QuizQuestionDto>> GetQuizQuestions(int quizId)
    {
        await _unitOfWork.Quizzes.GetByIdChecked(quizId);

        var quizQuestions = await _unitOfWork.QuizQuestions
            .Get(i => i.QuizId == quizId, includes: q => q.Choices);
        return quizQuestions.ToDto();
    }
}
