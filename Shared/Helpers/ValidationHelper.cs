using System.ComponentModel.DataAnnotations;

namespace Shared.Helpers;

public static class ValidationHelper
{
    public delegate T Formatter<T>(ICollection<ValidationResult> validationResults);

    public static object DefaultFormatter(ICollection<ValidationResult> validationResults) =>
        JsonFormatter(validationResults);

    /*
     *
     ****/
    public static void Validate<T>(T obj)
    {
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));

        Validator.ValidateObject(obj, new ValidationContext(obj), true);
    }

    /*
     *
     ****/
    public static bool TryValidate<T>(T obj, out List<ValidationResult> validationResults)
    {
        validationResults = [];
        if (obj is null)
        {
            validationResults.Add(new ValidationResult("Object is null"));
            return false;
        }
        return Validator.TryValidateObject(
            obj,
            new ValidationContext(obj),
            validationResults,
            true
        );
    }

    public static object FormatResults(ICollection<ValidationResult> validationResults) =>
        DefaultFormatter(validationResults);

    /*
     *
     ****/
    public static T FormatResults<T>(
        ICollection<ValidationResult> validationResults,
        Formatter<T> formatter
    ) => formatter(validationResults);

    /*
     *
     ****/
    public static object JsonFormatter(ICollection<ValidationResult> validationResults) =>
        validationResults
            .SelectMany(result =>
                result.MemberNames.Select(member => new
                {
                    property = member,
                    errors = new[] { result.ErrorMessage }
                })
            )
            .GroupBy(x => x.property)
            .Select(g => new { property = g.Key, errors = g.SelectMany(x => x.errors).ToArray() })
            .ToArray();

    /*
     *
     ****/
    public static string HumanReadableFormatter(List<ValidationResult> validationResults) =>
        string.Join(
            Environment.NewLine,
            validationResults.SelectMany(result =>
                result.MemberNames.Select(member =>
                    $"Property: {member} failed validation. Error: {result.ErrorMessage}"
                )
            )
        );
}
