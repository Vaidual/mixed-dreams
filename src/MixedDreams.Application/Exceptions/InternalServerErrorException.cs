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
        public const string GeneralTitle = "An internal server error occured.";
        public InternalServerErrorException(string title = GeneralTitle) : base(title)
        {
            Title = title;
        }

        public override int StatusCode { get; } = 500;
        public override string Title { get; init; }
    }
}
