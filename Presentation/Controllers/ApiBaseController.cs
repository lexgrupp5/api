using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

/// <summary>
/// A base controller that can be used for reusable
/// code in controllers actions.
/// </summary>
public class ApiBaseController : ControllerBase
{
    /// <summary>
    /// A reusable validation method that can be used to validate
    /// patch requests before patch is applied to persistent storage
    /// </summary>
    protected bool TryValidateAndApplyPatch<ModelType>(
        JsonPatchDocument<ModelType> patchDocument,
        ModelType model,
        out IActionResult errorResponse
    ) where ModelType : class
    {
        errorResponse = NotFound();
        if (model == null) { return false; }

        patchDocument.ApplyTo(model, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(model))
        {
            var errors = ModelState.Values
                .SelectMany(modelStateEntry => modelStateEntry.Errors)
                .Select(modelError => modelError.ErrorMessage)
                .ToList();

            errorResponse = BadRequest(new
            {
                Message = "Invalid patch state",
                Errors = errors
            });
            
            return false;
        }

        return true;
    }
}
