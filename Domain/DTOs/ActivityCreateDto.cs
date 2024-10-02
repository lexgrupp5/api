namespace Domain.DTOs;

#nullable disable

public class ActivityCreateDto
{
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Module
    public int ModuleId { get; set; }

    // Activity Type
    public string ActivityTypeName { get; set; }
}
