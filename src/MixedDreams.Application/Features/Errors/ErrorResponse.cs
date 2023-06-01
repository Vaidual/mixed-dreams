using MixedDreams.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.Errors
{
    public class ErrorResponse
    {

        public ErrorResponse(int statusCode, string title, ErrorCodes errorCode)
        {
            StatusCode = statusCode;
            Title = title;
            ErrorCode = errorCode;
        }

        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("errorCode")]
        public ErrorCodes ErrorCode { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
