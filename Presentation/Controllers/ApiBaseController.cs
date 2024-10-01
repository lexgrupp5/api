using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

public class ApiBaseController : ControllerBase
{
    /// <summary>
    /// A reusable validation method that can be used to validate
    /// patch requests before patch is applied to persistent storage
    /// </summary>
    protected bool TryValidateAndApplyPatch<TModel>(
        JsonPatchDocument<TModel> patchDocument,
        TModel model,
        out IActionResult errorResponse
    )
        where TModel : class
    {
        errorResponse = NotFound();
        if (model == null)
        {
            return false;
        }

        patchDocument.ApplyTo(model, ModelState);

        if (!ModelState.IsValid || !TryValidateModel(model))
        {
            var errors = ModelState
                .Values.SelectMany(modelStateEntry => modelStateEntry.Errors)
                .Select(modelError => modelError.ErrorMessage)
                .ToList();

            errorResponse = BadRequest(new { Message = "Invalid patch state", Errors = errors });

            return false;
        }

        return true;
    }
}
