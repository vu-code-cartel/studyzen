using System.Security.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyZen.Api.Extensions;
using StudyZen.Application.Dtos;
using StudyZen.Application.Services;

namespace StudyZen.Api.Controllers;

[ApiController]
[Route("[controller]")]

public sealed class AccountController : ControllerBase
{
    private readonly IApplicationUserService _applicationUserService;
    public readonly CookieOptions cookieOptions;

    public AccountController(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
        cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddDays(1)
        };
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterApplicationUser([FromBody] RegisterApplicationUserDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));

        await _applicationUserService.CreateApplicationUser(request);

        return Ok();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginApplicationUser([FromBody] LoginApplicationUserDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));

        var tokens = await _applicationUserService.AuthenticateApplicationUser(request);

        Response.Cookies.Append("AccessToken", tokens.AccessToken, cookieOptions);
        Response.Cookies.Append("RefreshToken", tokens.RefreshToken, cookieOptions);

        return Ok();
    }

    [HttpPost]
    [Route("refresh-tokens")]
    public async Task<IActionResult> RefreshApplicationUserTokens()
    {
        var refreshToken = Request.Cookies["RefreshToken"];
        refreshToken.ThrowIfRequestArgumentNull(nameof(refreshToken));

        var tokens = await _applicationUserService.RefreshApplicationUserTokens(refreshToken!);

        Response.Cookies.Append("AccessToken", tokens.AccessToken, cookieOptions);
        Response.Cookies.Append("RefreshToken", tokens.RefreshToken, cookieOptions);

        return Ok();
    }

    [HttpPost]
    [Route("logout")]
    public IActionResult LogoutApplicationUser()
    {
        Response.Cookies.Delete("AccessToken");
        Response.Cookies.Delete("RefreshToken");

        return Ok();
    }
    [HttpGet]
    [Route("user")]
    [Authorize(Roles = "Lecturer, Student")]
    public async Task<IActionResult> GetUser()
    {
        var applicationUser = await _applicationUserService.GetUser();
        return Ok(applicationUser);
    }
}