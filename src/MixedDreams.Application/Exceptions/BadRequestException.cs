using MixedDreams.Application.Common;
using MixedDreams.Application.Features.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MixedDreams.Application.Exceptions
{
    public class BadRequestException : BaseException
    {
        public override int StatusCode { get; } = 400;
        public override string Title { get; init; }
        public IEnumerable<string> Errors { get; set; }
        public BadRequestException(string title, IEnumerable<string> errors) : base(title) 
        { 
            Title = title;
            Errors = errors;
        }

        public override ErrorResponse GetErrorResponse()
        {
            return new BadRequestResponse(StatusCode, Title, Errors);
        }
    }
}
