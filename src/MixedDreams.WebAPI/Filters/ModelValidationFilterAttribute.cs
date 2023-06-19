using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.Features.Errors;
using MixedDreams.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using MixedDreams.Domain.Common;
using MixedDreams.Application.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using MixedDreams.Application.Exceptions.BadRequest;
using MixedDreams.Application.Enums;

namespace MixedDreams.WebAPI.Filters
{
    public class ModelValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //if (!context.ModelState.IsValid)
            //{
            //    var errors = context.ModelState
            //        .Where(x => x.Value.Errors.Count > 0)
            //        .SelectMany(x => x.Value.Errors)
            //        .Select(e => e.ErrorMessage);
            //    if (errors.Count() == 0) return;
            //    throw new BadRequestException("Json parsing is failed.", ErrorCodes.ParsingError, errors);
            //}
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
