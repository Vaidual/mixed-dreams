using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MixedDreams.Infrastructure.Enums;

namespace MixedDreams.Infrastructure.Features.Errors
{
    public class BadRequestResponse : ErrorResponse
    {
        [JsonPropertyName("errors")]
        public IEnumerable<string> Errors { get; set; }
        public BadRequestResponse(string title, IEnumerable<string> errors, ErrorCodes errorCode) : base(StatusCodes.Status400BadRequest, title, errorCode)
        {
            Errors = errors;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
