using Domain.DTOs;

namespace Domain.Validations;

public class DomainDateValidation
{
    public static bool IsValidDateCombination (DateTime startDate, DateTime endDate)
    {
        return endDate < startDate;
    }
}
