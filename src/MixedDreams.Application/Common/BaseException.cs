﻿using Microsoft.Extensions.Logging;
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

        protected BaseException(string title, int statusCode) : base(title) 
        { 
            Title = title;
            StatusCode = statusCode;
        }

        public virtual string GetResponse()
        {
            return JsonSerializer.Serialize(new ErrorResponse(StatusCode, Title));
        }
    }
}
