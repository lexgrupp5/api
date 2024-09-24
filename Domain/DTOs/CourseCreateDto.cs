using Domain.Validations;

namespace Domain.DTOs;

public class CourseCreateDto : ICourseDto
{
    public string Name { get; set; } = String.Empty;

    public string Description { get; set; } = String.Empty;

    public DateTime StartDate { get; set; }

    [EndDateValidation(nameof(StartDate))]
    public DateTime EndDate { get; set; }
}
