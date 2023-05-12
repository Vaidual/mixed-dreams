using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.Errors
{
    public class PropertyIsTakenBadRequestResponse : ErrorResponse
    {
        public PropertyIsTakenBadRequestResponse(string property, string value) : base(400, $"Property {property} with value {value} already exists.")
        {
        }
    }
}
