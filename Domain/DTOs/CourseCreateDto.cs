using Domain.Validations;
using V = Domain.Validations.DomainTextValidationAttribute;

namespace Domain.DTOs;

public class CourseCreateDto : ICourseDto
{
    [DomainTextValidation(V.DEFAULT_MIN, V.TITLE_MAX)]
    public string Name { get; set; } = String.Empty;

    [DomainTextValidation(V.DEFAULT_MIN, V.DESCRIPTION_MAX)]
    public string Description { get; set; } = String.Empty;

    public DateTime StartDate { get; set; }

    [EndDateValidation(nameof(StartDate))]
    public DateTime EndDate { get; set; }
}
