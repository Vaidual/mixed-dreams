using MixedDreams.Application.Common;
using MixedDreams.Application.Features.Errors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MixedDreams.Application.Exceptions
{
    public class BadRequestException : BaseException
    {
        [JsonPropertyName("errors")]
        public IEnumerable<string> Errors { get; set; }
        public BadRequestException(string title, IEnumerable<string> errors) : base(title, 400) 
        { 
            Errors = errors;
        }

        public override ErrorResponse GetErrorResponse()
        {
            return new BadRequestResponse(Title, Errors);
        }

        public override string GetResponse()
        {
            return JsonSerializer.Serialize(new { Errors, StatusCode, Title });
        }
    }
}
