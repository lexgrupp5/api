namespace Domain.DTOs;

public class CourseCreateDto
{
    public string Name { get; set; } = String.Empty;

    public string Description { get; set; } = String.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
