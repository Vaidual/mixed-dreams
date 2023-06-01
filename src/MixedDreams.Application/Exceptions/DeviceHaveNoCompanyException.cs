using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Exceptions
{
    public class DeviceHaveNoCompanyException : Exception
    {
        public DeviceHaveNoCompanyException(string deviceId) : base($"Device '${deviceId}' has no company however sent notification about request.")
        {
        }
    }
}
