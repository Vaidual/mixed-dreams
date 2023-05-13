using MixedDreams.Application.Features.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixedDreams.Application.Common
{
    public abstract class BaseException : Exception
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; private set; }

        [JsonPropertyName("title")]
        public string Title { get; private set; }

        protected BaseException(string title, int statusCode) : base(title) 
        { 
            Title = title;
            StatusCode = statusCode;
        }
        public virtual ErrorResponse GetErrorResponse()
        {
            return new ErrorResponse(StatusCode, Title);
        }

        public virtual string GetResponse()
        {
            return JsonSerializer.Serialize(new { StatusCode, Title });
        }
    }
}
