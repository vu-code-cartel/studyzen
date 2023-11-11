using Microsoft.AspNetCore.Mvc;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class QuizGamesController : ControllerBase
{
    private readonly IQuizGameService _quizGameService;

    public QuizGamesController(IQuizGameService quizGameService)
    {
        _quizGameService = quizGameService;
    }

    [HttpPost]
    [Route("{quizId}")]
    public async Task<IActionResult> CreateGame(int quizId)
    {
        var dto = await _quizGameService.CreateGame(quizId);
        return Ok(dto);
    }

    [HttpPost]
    [Route("{gamePin}/Join")]
    public async Task<IActionResult> JoinGame(string gamePin, string username, string connectionId)
    {
        var dto = new JoinQuizGameDto(gamePin, username, connectionId);
        await _quizGameService.JoinGame(dto);
        return Ok();
    }

    [HttpGet]
    [Route("{gamePin}")]
    public Task<IActionResult> GetGame(string gamePin)
    {
        var game = _quizGameService.GetGame(gamePin);
        IActionResult result = Ok(game);
        return Task.FromResult(result);
    }

    [HttpPost]
    [Route("{gamePin}/Start")]
    public async Task<IActionResult> StartGame(string gamePin)
    {
        _quizGameService.StartGame(gamePin);
        await _quizGameService.SendNextQuestion(gamePin);
        return Ok();
    }
}
