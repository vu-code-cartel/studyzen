using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using StudyZen.Application.Dtos;
using StudyZen.Application.Validation;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public sealed class ApplicationUserService : IApplicationUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ValidationHandler _validationHandler;
    private readonly ITokenManagementService _tokenManagementService;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public ApplicationUserService(UserManager<ApplicationUser> userManager, ValidationHandler validationHandler, ITokenManagementService tokenManagementService, ICurrentUserAccessor currentUserAccessor)
    {
        _userManager = userManager;
        _validationHandler = validationHandler;
        _tokenManagementService = tokenManagementService;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task CreateApplicationUser(RegisterApplicationUserDto dto)
    {
        await _validationHandler.ValidateAsync(dto);

        var applicationUser = new ApplicationUser
        {
            UserName = dto.Username,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        var result = await _userManager.CreateAsync(applicationUser, dto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(applicationUser, dto.Role);
        }
    }

    public async Task<Tokens> AuthenticateApplicationUser(LoginApplicationUserDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is not null && await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            return await GetTokens(user);
        }

        else
        {
            throw new AuthenticationException("Invalid email or password");
        }
    }

    public async Task<Tokens> RefreshApplicationUserTokens(string token)
    {
        var refreshToken = await _tokenManagementService.GetRefreshToken(token);
        if (refreshToken is null || refreshToken.IsRevoked || refreshToken.ExpiryDate < DateTime.UtcNow)
        {
            throw new AuthenticationException();
        }
        else
        {
            var applicationUser = await _userManager.FindByIdAsync(refreshToken!.ApplicationUserId);

            await _tokenManagementService.RevokeRefreshToken(refreshToken);

            return await GetTokens(applicationUser!);
        }
    }

    private async Task<Tokens> GetTokens(ApplicationUser applicationUser)
    {
        var accessToken = await _tokenManagementService.CreateAccessToken(applicationUser);
        var refreshToken = await _tokenManagementService.CreateRefreshToken(applicationUser);
        return new Tokens(accessToken, refreshToken);
    }

    public async Task<ApplicationUserDto> GetUser()
    {
        var applicationUserId = _currentUserAccessor.GetUserId();
        var applicationUser = await _userManager.FindByIdAsync(applicationUserId);
        var role = _userManager.GetRolesAsync(applicationUser!).Result.FirstOrDefault();

        return new ApplicationUserDto(applicationUser!, role!);
    }
}