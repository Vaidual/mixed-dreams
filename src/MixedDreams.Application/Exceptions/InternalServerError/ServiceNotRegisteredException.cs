using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Exceptions.InternalServerError
{
    public class ServiceNotRegisteredException : InternalServerErrorException
    {
        public ServiceNotRegisteredException(string serviceName) : base($"Service '{serviceName} isn't registered in the container.'")
        {
        }
    }
}
