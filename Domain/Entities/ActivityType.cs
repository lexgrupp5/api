namespace Domain.Entities;

#nullable disable

public class ActivityType
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    // Navigation
    public ICollection<Activity> Activities { get; } = [];
}
