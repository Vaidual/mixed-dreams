using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Exceptions.InternalServerError
{
    public class ClaimDoesntExistException : InternalServerErrorException
    {
        public ClaimDoesntExistException(string claimType) : base($"Claim with type '{claimType} wasn't found'") { }
    }
}
