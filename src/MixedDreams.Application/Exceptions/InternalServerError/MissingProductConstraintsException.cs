using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions.InternalServerError
{
    public class MissingProductConstraintsException : InternalServerErrorException
    {
        public MissingProductConstraintsException(string productid) : base($"Product constraint for product '${productid}' were asked but they are not specified.")
        {
        }
    }
}
