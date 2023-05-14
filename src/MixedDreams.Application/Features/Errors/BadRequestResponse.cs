using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MixedDreams.Application.Features.Errors
{
    public class BadRequestResponse : ErrorResponse
    {
        [JsonPropertyName("errors")]
        public IEnumerable<string> Errors { get; set; }
        public BadRequestResponse(string title, IEnumerable<string> errors) : base(StatusCodes.Status400BadRequest, title)
        {
            Errors = errors;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
