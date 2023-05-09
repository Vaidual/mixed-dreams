using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.Errors
{
    public class EntityNotFoundResponse : ErrorResponse
    {
        public EntityNotFoundResponse(string entityName, string key) : base(404, $"{entityName} with identifier {key} doesn't exist.")
        {
        }
    }
}
