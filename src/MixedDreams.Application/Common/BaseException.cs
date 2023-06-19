using Microsoft.Extensions.Logging;
using MixedDreams.Application.Enums;
using MixedDreams.Application.Features.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Common
{
    public abstract class BaseException : Exception
    {
        public abstract LogLevel LogLevel { get; init; }

        protected BaseException(string message) : base(message) { }
    }
}
