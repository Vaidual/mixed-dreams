using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.Errors
{
    public class PutNotFoundResponse : ErrorResponse
    {
        public PutNotFoundResponse() : base(404, "Entiry to update not found. For creation of a new record use POST method instead")
        {
        }
    }
}
