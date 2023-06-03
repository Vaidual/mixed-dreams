using Microsoft.Extensions.Logging;
using MixedDreams.Application.Common;
using MixedDreams.Infrastructure.Enums;
using MixedDreams.Infrastructure.Features.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Common
{
    public abstract class BaseHttpException : BaseException
    {
        public int StatusCode { get; private set; }
        public string Title { get; private set; }
        public ErrorCodes ErrorCode { get; init; }

        protected BaseHttpException(string title, int statusCode, ErrorCodes errorCode) : base(title) 
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
