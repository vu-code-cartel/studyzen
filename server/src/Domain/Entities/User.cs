using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StudyZen.Domain.Constraints;

namespace StudyZen.Domain.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(UserConstraints.UserNameMaxLength)]
    public string Username { get; set; }

    [Required]
    public string HashedPassword { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpiryTime { get; set; }

    public User(string username, string hashedPassword)
    {
        Username = username;
        HashedPassword = hashedPassword;
    }

    public void SetRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = refreshTokenExpiryTime;
    }

}