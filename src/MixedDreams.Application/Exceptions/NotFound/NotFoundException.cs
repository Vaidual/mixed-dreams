using Microsoft.Extensions.Logging;
using MixedDreams.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions.NotFound
{
    public class NotFoundException : BaseException
    {
        public override LogLevel LogLevel { get; init; } = LogLevel.None;
        public NotFoundException(string title) : base(title, 404) { }
    }
}
