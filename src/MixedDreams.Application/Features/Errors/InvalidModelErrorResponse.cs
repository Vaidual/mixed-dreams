using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.Errors
{
    public class InvalidModelErrorResponse : ErrorResponse
    {
        [JsonPropertyName("errors")]
        public IDictionary<string, IEnumerable<string>> Errors { get; set; }
        public InvalidModelErrorResponse(string title, IDictionary<string, IEnumerable<string>> errors) : base(StatusCodes.Status422UnprocessableEntity, title)
        {
            Errors = errors;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
