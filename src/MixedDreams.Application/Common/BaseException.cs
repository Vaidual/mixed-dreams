using MixedDreams.Application.Features.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Common
{
    public abstract class BaseException : Exception
    {
        public abstract int StatusCode { get; }
        public abstract string Title { get; init; }
        protected BaseException(string Title) : base(Title) { }
        public virtual ErrorResponse GetErrorResponse()
        {
            return new ErrorResponse(StatusCode, Title);
        }
    }
}
