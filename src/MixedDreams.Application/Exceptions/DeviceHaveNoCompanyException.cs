using Microsoft.Extensions.Logging;
using MixedDreams.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Exceptions
{
    public class DeviceHaveNoCompanyException : BaseException
    {
        public DeviceHaveNoCompanyException(string deviceId) : 
            base($"Device '${deviceId}' has no company however sent notification about request.")
        { }

        public override LogLevel LogLevel { get; init; } = LogLevel.Error;
    }
}
