using MixedDreams.Application.Common;
using MixedDreams.Application.Features.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions
{
    public class ModelValidationException : BaseException
    {
        [JsonPropertyName("errors")]
        public IDictionary<string, IEnumerable<string>> Errors { get; set; }

        public ModelValidationException(IDictionary<string, IEnumerable<string>> errors) : base("One or more validation failures have occurred.", 422)
        {
            Errors = errors;
        }

        public override ErrorResponse GetErrorResponse()
        {
            return new InvalidModelErrorResponse(StatusCode, Title, Errors);
        }

        public override string GetResponse()
        {
            return JsonSerializer.Serialize(new { Errors, StatusCode, Title });
        }
    }
}
