namespace Domain.DTOs;

public class ActivityForCreationDto
{
    
    public int ModuleId { get; set; }
    
    public string Description { get; set; } = String.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}