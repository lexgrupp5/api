namespace Domain.Entities;

#nullable disable

public class Activity
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    //Navigation
    public Module Module { get; set; }

    public ActivityType ActivityType { get; set; }

    private ICollection<Document> Documents { get; set; }
}
