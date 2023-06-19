using MixedDreams.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions.BadRequest
{
    public class PutNotFoundException : BadRequestException
    {
        private const string GeneralTitle = "Entiry to update not found. For creation of a new record use POST method instead";
        public PutNotFoundException() : base(GeneralTitle, ErrorCodes.InternalError)
        {
        }
    }
}
