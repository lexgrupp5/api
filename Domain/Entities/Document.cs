namespace Domain.Entities;

#nullable disable

public class Document
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public DateTime Timestamp { get; set; }
    
    public string Description { get; set; }
    
    public string UploadedBy { get; set; }
    
    public User User { get; set; }
    
    public Course Course { get; set; }
    
    public Module Module { get; set; }
    
    public Activity Activity { get; set; }
}