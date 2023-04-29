using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixedDreams.Core.Responses.Errors
{
    public class InvalidModelError
    {
        public InvalidModelError(string property, IEnumerable<string> errors)
        {
            Property = property;
            Errors = errors;
        }

        public string Property { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
    public class InvalidModelErrorResponse : ErrorResponse
    {

        [JsonPropertyName("errors")]
        public object Errors { get; set; }
        public InvalidModelErrorResponse(int statusCode, string title, object errors) : base(statusCode, title)
        {
            Errors = errors;
        }
    }
}
