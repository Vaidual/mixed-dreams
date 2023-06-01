using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MixedDreams.Infrastructure.Common;
using MixedDreams.Infrastructure.Enums;
using MixedDreams.Infrastructure.Features.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Exceptions
{
    public class ModelValidationException : BaseHttpException
    {
        [JsonPropertyName("errors")]
        public IDictionary<string, IEnumerable<string>> Errors { get; set; }

        public override LogLevel LogLevel { get; init; } = LogLevel.None;

        public ModelValidationException(IDictionary<string, IEnumerable<string>> errors) : base("One or more validation failures have occurred.", StatusCodes.Status422UnprocessableEntity, ErrorCodes.ModelValidationError)
        {
            Errors = errors;
        }

        public override string GetResponse()
        {
            return JsonSerializer.Serialize(new InvalidModelErrorResponse(Title, Errors));
        }
    }
}
