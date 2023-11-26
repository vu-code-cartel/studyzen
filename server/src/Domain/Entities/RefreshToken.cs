using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace StudyZen.Domain.Entities;

[ExcludeFromCodeCoverage]
public class RefreshToken : BaseEntity
{
    [Required]
    public string RefreshTokenHash { get; set; } = null!;

    [Required]
    public string ApplicationUserId { get; set; } = null!;

    [ForeignKey(nameof(ApplicationUserId))]
    public ApplicationUser ApplicationUser { get; set; } = null!;

    [Required]
    public DateTime ExpiryDate { get; set; }

    [Required]
    public bool IsRevoked { get; set; }

    public RefreshToken(string refreshTokenHash, string applicationUserId, DateTime expiryDate)
    {
        RefreshTokenHash = refreshTokenHash;
        ApplicationUserId = applicationUserId;
        ExpiryDate = expiryDate;
        IsRevoked = false;
    }
}