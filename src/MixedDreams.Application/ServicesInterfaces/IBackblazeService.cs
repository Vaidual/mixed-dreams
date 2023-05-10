using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.ServicesInterfaces
{
    public interface IBackblazeService
    {
        public Task<string> UploadImage(IFormFile image);
    }
}
