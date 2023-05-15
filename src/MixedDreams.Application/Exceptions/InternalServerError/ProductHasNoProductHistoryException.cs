using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions.InternalServerError
{
    public class ProductHasNoProductHistoryException : InternalServerErrorException
    {
        public ProductHasNoProductHistoryException(string productId) : base($"Product with id '{productId}' have no producthistory records")
        {
        }
    }
}
