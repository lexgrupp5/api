//
//namespace Domain.Validations;
//
//using System.ComponentModel.DataAnnotations;
//
//using Domain.DTOs;
//
//public class DomainTextValidationAttribute : ValidationAttribute
//{
//    private readonly int _minLength;
//    private readonly int _maxLength;
//    public const int DEFAULT_MIN = 1;
//    public const int DEFAULT_MAX = 1000; 
//    public const int TITLE_MAX = 400;
//    public const int DESCRIPTION_MAX = 4000;
//
//    public DomainTextValidationAttribute(
//        int minLength = DEFAULT_MIN,
//        int maxLength = DEFAULT_MAX)
//    {
//        _minLength = minLength;
//        _maxLength = maxLength;
//    }
//
//    protected override ValidationResult? IsValid(
//        object? value, ValidationContext validationContext)
//    {
//        if (value is not string input||
//            validationContext.ObjectInstance is not ICourseDto)
//        {
//            return new ValidationResult($"Validation {nameof(DomainTextValidationAttribute)} error");
//        }
//
//        if (input.Length < _minLength || input.Length > _maxLength)
//        {
//            var msg = string.IsNullOrEmpty(ErrorMessage)
//                ? $"{validationContext.DisplayName} must be between" +
//                $"[{_minLength}, {_maxLength}] characters."
//                : ErrorMessage;
//
//            return new ValidationResult(msg);
//        }
//
//        return ValidationResult.Success;
//    }
//}
//