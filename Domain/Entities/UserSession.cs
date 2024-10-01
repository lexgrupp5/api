namespace Domain.Entities;

#nullable disable

public class UserSession
{
    public int Id { get; set; }
    
    public string RefreshToken { get; set; }

    public DateTime ExpiresAt { get; set; }
    
    public string UserId { get; set; }

    // Navigation
    public User User { get; set; }
}