using System.ComponentModel.DataAnnotations;

namespace Domain.Validations;

public class DomainDateValidationAttribute : ValidationAttribute
{
    public const int MIN_YEAR_FROM_CURRENT_YEAR = -10;

    public override bool IsValid(
    object? value)
    {
        if (value is not DateTime input)
        {
            return false;
        }

        return input >= DateTime.Now.AddYears(MIN_YEAR_FROM_CURRENT_YEAR);
    }
}
