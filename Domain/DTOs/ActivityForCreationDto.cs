namespace Domain.DTOs;

public class ActivityForCreationDto
{
    public string Description { get; set; } = String.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}