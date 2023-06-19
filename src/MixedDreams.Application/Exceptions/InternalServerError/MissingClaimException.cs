using MixedDreams.Application.Exceptions.InternalServerError;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions.InternalServerError
{
    public class MissingClaimException : InternalServerErrorException
    {
        public MissingClaimException(string claim) : base($"User missing claim ${claim}.")
        {
        }
    }
}
