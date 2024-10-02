namespace Domain.DTOs;

#nullable disable

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string TeacherId { get; set; }
}


/*
using Domain.Validations;
using V = Domain.Validations.DomainTextValidationAttribute;
 */


/*
public record CourseDto(
    int Id,
    [DomainTextValidation(V.DEFAULT_MIN, V.TITLE_MAX)] string Name,
    [DomainTextValidation(V.DEFAULT_MIN, V.DESCRIPTION_MAX)] string Description,
    DateTime StartDate,
    [EndDateValidation(nameof(StartDate))] DateTime EndDate,
    string[] ModuleNames
) : ICourseDto;
 */
