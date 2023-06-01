using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Enums
{
    public enum ErrorCodes
    {
        InternalError = -1,

        EmailIsTaken = 0,
        LargeOrder = 1,
        PeriodDoesntExist = 2,
        PropertyIsTaken = 3,
        EntityNotFound = 4,
        Invalidcredentials  = 5,
        ParsingError = 6,
        ModelValidationError = 6,
    }
}
