using Domain.Validations;

namespace Domain.DTOs
{
    public record CourseDto(
        int Id,
        string Name,
        string Description,
        DateTime StartDate,
        [EndDateValidation(nameof(StartDate))] DateTime EndDate
    ) : ICourseDto;
}
