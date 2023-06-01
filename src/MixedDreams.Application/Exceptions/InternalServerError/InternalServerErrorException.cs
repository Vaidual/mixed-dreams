using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MixedDreams.Infrastructure.Common;
using MixedDreams.Infrastructure.Enums;
using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Exceptions.InternalServerError
{
    public class InternalServerErrorException : BaseHttpException
    {
        public override LogLevel LogLevel { get; init; } = LogLevel.Error;
        public InternalServerErrorException(string title) : base(title, StatusCodes.Status500InternalServerError, ErrorCodes.InternalError) { }
    }
}
