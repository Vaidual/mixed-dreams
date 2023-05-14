using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MixedDreams.Application.Common;
using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions
{
    public class InternalServerErrorException : BaseException
    {
        public override LogLevel LogLevel { get; init; } = LogLevel.Error;
        public InternalServerErrorException(string title) : base(title, StatusCodes.Status500InternalServerError) { }
    }
}
