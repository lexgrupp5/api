using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Helpers;

namespace Presentation.Filters;

public class ValidateInputAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var isSkipValidation =
            (context.ActionDescriptor as ControllerActionDescriptor)
                ?.MethodInfo.GetCustomAttributes<SkipValidationAttribute>(true)
                .Any() ?? false;

        if (isSkipValidation)
            return;

        var validationResults = new List<ValidationResult>();
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            var isValid = ValidationHelper.TryValidate(argument, out var results);
            if (!isValid)
                validationResults.AddRange(results);
        }

        if (validationResults.Count > 0)
            context.Result = new BadRequestObjectResult(
                ValidationHelper.FormatResults(validationResults)
            );
    }
}
