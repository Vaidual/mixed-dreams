using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions.BadRequest
{
    public class LargeOrderException : BadRequestException
    {
        private const int maxOrderSize = 50;
        public LargeOrderException() : base($"You can't but more than {maxOrderSize} products. Contact the seller, If you wish to place a larger order.")
        {
        }
    }
}
