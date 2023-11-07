using Microsoft.AspNetCore.Identity;
using StudyZen.Application.Dtos;
using StudyZen.Application.Validation;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public sealed class ApplicationUserService : IApplicationUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ValidationHandler _validationHandler;


    public ApplicationUserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ValidationHandler validationHandler)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _validationHandler = validationHandler;
    }

    public async Task<ApplicationUserDto> CreateApplicationUser(RegisterApplicationUserDto dto)
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

        return new ApplicationUserDto(applicationUser);
    }
}