using System.ComponentModel.DataAnnotations;
using Domain.DTOs;

namespace Domain.Validations;

public class EndDateValidationAttribute : ValidationAttribute
{
    private readonly string _startDatePropertyName;

    public EndDateValidationAttribute(string startDatePropertyName)
    {
        _startDatePropertyName = startDatePropertyName;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateTime endDate ||
            validationContext.ObjectInstance is not ICourseDto)
        {
            return new ValidationResult($"Validation {nameof(EndDateValidationAttribute)} error");
        }
        
        var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);

        if (startDateProperty == null)
        {
            return new ValidationResult($"Unknown property: {_startDatePropertyName}");
        }

        if (startDateProperty.GetValue(
            validationContext.ObjectInstance) is not DateTime startDate)
        {
            return new ValidationResult($"Validation {nameof(EndDateValidationAttribute)} error");
        }

        if (!IsValidDateCombination(startDate, endDate))
        {
            return new ValidationResult("EndDate must be later than StartDate.");
        }

        return ValidationResult.Success;
    }

    public static bool IsValidDateCombination (DateTime startDate, DateTime endDate)
    {
        return startDate < endDate;
    }
}
