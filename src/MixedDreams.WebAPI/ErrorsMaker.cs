using FluentValidation.Results;
using MixedDreams.Application.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace MixedDreams.WebAPI
{
    public class ErrorsMaker
    {
        public static void ProcessValidationErrors(List<ValidationFailure> failures)
        {
            Dictionary<string, string[]> errors = failures
                .ToDictionary(
                    x => x.PropertyName,
                    x => new string[] { x.ErrorMessage }
                );
            throw new ModelValidationException(errors);
        }
    }
}
