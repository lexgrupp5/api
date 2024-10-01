using Domain.Validations;

namespace Domain.DTOs;

public class SearchFilterDTO
{
    [SearchTextValidation]
    public string SearchText { get; set; } = string.Empty;

    public DateTime EndDate { get; set; } = DateTime.MaxValue;

    public DateTime StartDate { get; set; } = DateTime.MinValue;
}
