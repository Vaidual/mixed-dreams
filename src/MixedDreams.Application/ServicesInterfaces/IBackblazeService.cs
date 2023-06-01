using Microsoft.AspNetCore.Http;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Hubs.Clients
{
    public interface IBackblazeService
    {
        public Task<BackblazeFile> UploadImage(IFormFile image);
        public Task DeleteImage(string field, string fileName);
    }
}
