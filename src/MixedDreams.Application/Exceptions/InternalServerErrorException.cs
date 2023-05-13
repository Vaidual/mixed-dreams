using MixedDreams.Application.Common;
using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions
{
    public class InternalServerErrorException : BaseException
    {
        public InternalServerErrorException(string title) : base(title, 500) { }
    }
}
