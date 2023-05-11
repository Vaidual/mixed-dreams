using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.Errors
{
    public class BadRequestResponse : ErrorResponse
    {
        [JsonPropertyName("errors")]
        public IEnumerable<string> Errors { get; set; }
        public BadRequestResponse(int statusCode, string title, IEnumerable<string> errors) : base(statusCode, title)
        {
            Errors = errors;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
