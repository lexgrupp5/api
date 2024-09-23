using Domain.Validations;

namespace Domain.DTOs;

public class SearchFilterDTO
{
    [SearchTextValidation]
    public string SearchText { get; set; } = string.Empty;

    public DateTime EndDate { get; set; } = DateTime.Now.AddYears(10);

    public DateTime StartDate { get; set; } = DateTime.Now.AddYears(-10);
}
