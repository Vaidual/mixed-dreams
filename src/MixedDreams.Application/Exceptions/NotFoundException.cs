using MixedDreams.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string title) : base(title, 404) { }
    }
}
