using System.ComponentModel.DataAnnotations;

using Domain.DTOs;

namespace Domain.Validations;

public class SearchTextValidationAttribute : ValidationAttribute
{
    public const int MIN_LENGTH = 1;

    public const int MAX_LENGTH = 100;
    
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not string input ||
            validationContext.ObjectInstance is not SearchFilterDTO dto)
        {
            return new ValidationResult($"Validation {nameof(SearchTextValidationAttribute)} error");
        }

        var msg = $"Search text must be in interval [{MIN_LENGTH}, {MAX_LENGTH}]";

        return (dto.SearchText.Length < MIN_LENGTH ||
            dto.SearchText.Length > MAX_LENGTH)
            ? new ValidationResult(msg)
            : ValidationResult.Success;
    }
}
