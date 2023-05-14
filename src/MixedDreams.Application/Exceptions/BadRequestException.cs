using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MixedDreams.Application.Common;
using MixedDreams.Application.Features.Errors;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MixedDreams.Application.Exceptions
{
    public class BadRequestException : BaseException
    {
        public IEnumerable<string> Errors { get; set; }

        public override LogLevel LogLevel { get; init; } = LogLevel.None;

        public BadRequestException(string title, IEnumerable<string>? errors = null) : base(title, 400) 
        { 
            Errors = errors ?? Enumerable.Empty<string>();
        }

        public override string GetResponse()
        {
            return JsonSerializer.Serialize(new BadRequestResponse(Title, Errors));
        }
    }
}
