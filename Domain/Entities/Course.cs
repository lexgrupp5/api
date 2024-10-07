namespace Domain.Entities;

public class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? TeacherId { get; set; }

    // Navigation
    public User? Teacher { get; set; }

    public ICollection<User> Users { get; set; } = [];

    public ICollection<Module> Modules { get; set; } = [];

    public ICollection<Document> Documents { get; set; } = [];
}
