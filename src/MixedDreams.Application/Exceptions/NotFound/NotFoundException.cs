using Microsoft.Extensions.Logging;
using MixedDreams.Infrastructure.Common;
using MixedDreams.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Exceptions.NotFound
{
    public class NotFoundException : BaseHttpException
    {
        public override LogLevel LogLevel { get; init; } = LogLevel.None;
        public NotFoundException(string title, ErrorCodes errorCode) : base(title, 404, errorCode) { }
    }
}
