using MixedDreams.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions.InternalServerError
{
    public class MissingConfigurationOptionsException : InternalServerErrorException
    {
        public MissingConfigurationOptionsException(string optionName) : base($"Option '{optionName}' isn't specified or options path is incorrect")
        {
        }
    }
}
