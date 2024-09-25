using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

#nullable disable

public class UserRefreshToken
{
    public int Id { get; set; }
    
    public string Token { get; set; }

    public DateTime ExpiresAt { get; set; }

    /* public DateTime CreatedAt { get; set; } */

    /* public bool IsRevoked { get; set; } */
    
    public string UserId { get; set; }

    // Navigation
    public User User { get; set; }
}