using MixedDreams.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Exceptions.BadRequest
{
    public class PeriodDoesntExistException : BadRequestException
    {
        public PeriodDoesntExistException(string period) : base($"Period with identifier '{period}' doesn't exist.", ErrorCodes.PeriodDoesntExist)
        {
        }
    }
}
