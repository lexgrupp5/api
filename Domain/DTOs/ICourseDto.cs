namespace Domain.DTOs;

public interface ICourseDto
{
    string Description { get; }
    DateTime EndDate { get; }
    string Name { get; }
    DateTime StartDate { get; }
}
