namespace Domain.Entities;

#nullable disable

public class Activity
{
    public int Id { get; set; }

    public string Description { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }

    public ActivityType ActivityTypeId { get; set; }
    
    public Module Module { get; set; }
    
    ICollection<Document> Documents { get; set; }
}