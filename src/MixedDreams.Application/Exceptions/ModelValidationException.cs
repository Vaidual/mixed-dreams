using MixedDreams.Application.Common;
using MixedDreams.Application.Dto.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions
{
    public class ModelValidationException : BaseException
    {
        public override int StatusCode { get; } = 422;
        public override string Title { get; init; }
        public IDictionary<string, string[]> Errors { get; set; }

        public ModelValidationException(IDictionary<string, string[]> errors) : base("One or more validation failures have occurred.")
        {
            Title = "One or more validation failures have occurred.";
            Errors = errors;
        }

        public override ErrorResponse GetErrorResponse()
        {
            return new InvalidModelErrorResponse(StatusCode, Title, Errors);
        }
    }
}
