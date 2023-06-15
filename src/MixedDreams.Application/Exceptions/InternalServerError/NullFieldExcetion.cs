using MixedDreams.Infrastructure.Exceptions.InternalServerError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions.InternalServerError
{
    public class NullFieldExcetion : InternalServerErrorException
    {
        public NullFieldExcetion(string field, string entity) : base($"Field ${field} was required in entity ${entity} but it's null.but  ")
        {
        }
    }
}
