using MixedDreams.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Exceptions.InternalServerError
{
    public class MissingConfigurationOptionsException : InternalServerErrorException
    {
        public MissingConfigurationOptionsException(string optionName) : base($"Option '{optionName}' isn't specified or options path is incorrect")
        {
        }
    }
}
