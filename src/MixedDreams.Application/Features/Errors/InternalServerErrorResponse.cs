using Microsoft.AspNetCore.Http;
using MixedDreams.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.Errors
{
    public class InternalServerErrorResponse : ErrorResponse
    {
        public const string GeneralTitle = "An internal server error occured.";
        public InternalServerErrorResponse() : base(StatusCodes.Status500InternalServerError, GeneralTitle, ErrorCodes.InternalError) { }
    }
}
