using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudyZen.Application.Repositories;
using StudyZen.Domain.Entities;

namespace StudyZen.Application.Services;

public class TokenManagementService : ITokenManagementService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public TokenManagementService(UserManager<ApplicationUser> userManager, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> CreateAccessToken(ApplicationUser applicationUser)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, applicationUser.Id),
            new Claim(ClaimTypes.Name, applicationUser.UserName!),
        };

        var userRoles = await _userManager.GetRolesAsync(applicationUser);
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };


        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> CreateRefreshToken(ApplicationUser applicationUser)
    {
        var refreshTokenValue = GenerateRefreshToken();
        var hashedRefreshToken = HashRefreshToken(refreshTokenValue);

        var refreshToken = new RefreshToken(hashedRefreshToken, applicationUser.Id, DateTime.UtcNow.AddMinutes(30));

        _unitOfWork.RefreshTokens.Add(refreshToken);
        await _unitOfWork.SaveChanges();

        return refreshTokenValue;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    private string HashRefreshToken(string refreshToken)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }

    public async Task<RefreshToken?> GetRefreshToken(string token)
    {
        var hashedToken = HashRefreshToken(token);

        var refreshToken = (await _unitOfWork.RefreshTokens.Get(
            filter: t => t.RefreshTokenHash == hashedToken,
            take: 1)).FirstOrDefault();

        return refreshToken;
    }

    public async Task RevokeRefreshToken(RefreshToken refreshToken)
    {
        refreshToken.IsRevoked = true;
        _unitOfWork.RefreshTokens.Update(refreshToken);
        await _unitOfWork.SaveChanges();
    }

}