namespace Domain.DTOs;

#nullable disable

public record CourseDto
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public DateTime? StartDate { get; init; } = null;
    public DateTime? EndDate { get; init; } = null;
    public string TeacherId { get; init; }
};







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
