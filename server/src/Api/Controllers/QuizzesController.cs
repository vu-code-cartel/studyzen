using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class QuizzesController : ControllerBase
{
    private readonly IQuizService _quizService;

    public QuizzesController(IQuizService quizService)
    {
        _quizService = quizService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuiz(CreateQuizDto dto)
    {
        dto.ThrowIfRequestArgumentNull(nameof(dto));
        var quiz = await _quizService.CreateQuiz(dto);
        return CreatedAtAction(nameof(GetQuiz), new { quizId = quiz.Id }, quiz);
    }

    [HttpGet]
    [Route("{quizId}")]
    public async Task<IActionResult> GetQuiz(int quizId)
    {
        var quiz = await _quizService.GetQuiz(quizId);
        return Ok(quiz);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllQuizzes()
    {
        var quizzes = await _quizService.GetAllQuizzes();
        return Ok(quizzes);
    }

    [HttpPatch]
    [Route("{quizId}")]
    public async Task<IActionResult> UpdateQuiz(int quizId, UpdateQuizDto dto)
    {
        dto.ThrowIfRequestArgumentNull(nameof(dto));
        await _quizService.UpdateQuiz(quizId, dto);
        return Ok();
    }

    [HttpDelete]
    [Route("{quizId}")]
    public async Task<IActionResult> DeleteQuiz(int quizId)
    {
        await _quizService.DeleteQuiz(quizId);
        return Ok();
    }

    [HttpPost]
    [Route("{quizId}/Questions")]
    public async Task<IActionResult> AddQuestionToQuiz(int quizId, CreateQuizQuestionDto dto)
    {
        dto.ThrowIfRequestArgumentNull(nameof(dto));
        var question = await _quizService.AddQuestionToQuiz(quizId, dto);
        return Ok(question);
    }

    [HttpGet]
    [Route("{quizId}/Questions")]
    public async Task<IActionResult> GetQuizQuestions(int quizId)
    {
        var questions = await _quizService.GetQuizQuestions(quizId);
        return Ok(questions);
    }
}
