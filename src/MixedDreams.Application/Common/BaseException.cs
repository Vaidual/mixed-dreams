using Microsoft.Extensions.Logging;
using MixedDreams.Application.Enums;
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
        public int StatusCode { get; private set; }
        public string Title { get; private set; }
        public abstract LogLevel LogLevel { get; init; }
        public ErrorCodes ErrorCode { get; init; }

        protected BaseException(string title, int statusCode, ErrorCodes errorCode) : base(title) 
        { 
            Title = title;
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }

        public virtual string GetResponse()
        {
            return JsonSerializer.Serialize(new ErrorResponse(StatusCode, Title, ErrorCode));
        }
    }
}
