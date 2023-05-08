using MixedDreams.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string title) : base(title) 
        { 
            Title = title;
        }

        public override int StatusCode { get; } = 400;
        public override string Title { get; init; }
    }
}
