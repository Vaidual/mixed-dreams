using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Core.Responses;
using MixedDreams.Core.Responses.Errors;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MixedDreams.WebAPI.Filters
{
    public class ModelValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(
                    new InvalidModelErrorResponse(
                        422, 
                        "Invalid request model", 
                        context.ModelState.Select(x => new InvalidModelError(
                            x.Key, 
                            x.Value.Errors.Select(e => e.ErrorMessage))
                        )
                    )
                );
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
