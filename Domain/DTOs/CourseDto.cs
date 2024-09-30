using Domain.Validations;
using V = Domain.Validations.DomainTextValidationAttribute;

namespace Domain.DTOs
{
    public record CourseDto(
        int Id,
        [DomainTextValidation(V.DEFAULT_MIN, V.TITLE_MAX)] string Name,
        [DomainTextValidation(V.DEFAULT_MIN, V.DESCRIPTION_MAX)] string Description,
        DateTime StartDate,
        [EndDateValidation(nameof(StartDate))] DateTime EndDate
    ) : ICourseDto;
}
