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
        public IDictionary<string, string[]> Errors { get; set; }
        public InvalidModelErrorResponse(int statusCode, string title, IDictionary<string, string[]> errors) : base(statusCode, title)
        {
            Errors = errors;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
