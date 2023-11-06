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

    public AccountController(IApplicationUserService applicationUserService)
    {
        _applicationUserService = applicationUserService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterApplicationUser([FromBody] RegisterApplicationUserDto request)
    {
        request.ThrowIfRequestArgumentNull(nameof(request));
        var newApplicationUser = await _applicationUserService.CreateApplicationUser(request);
        return Ok(newApplicationUser);
    }
}