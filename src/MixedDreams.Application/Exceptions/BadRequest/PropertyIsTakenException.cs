using MixedDreams.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Exceptions.BadRequest
{
    public class PropertyIsTakenException : BadRequestException
    {
        public PropertyIsTakenException(string property, string value) : base($"Property '{property}' with value '{value}' already exists.", ErrorCodes.PropertyIsTaken)
        {
        }
    }
}
