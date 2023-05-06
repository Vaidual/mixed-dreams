using Bytewizer.Backblaze.Client;
using Bytewizer.Backblaze.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Services
{
    internal class BackblazeService
    {
        private readonly IStorageClient _storage;
        private readonly IConfiguration _configuration;

        public BackblazeService(IStorageClient storage, IConfiguration configuration)
        {
            _storage = storage;
            this._configuration = configuration;
        }

        public async Task UploadImage(IFormFile image)
        {
            await _storage.UploadAsync(_configuration["Backblaze:MixedDreams"], image.FileName, image.OpenReadStream());
        }
    }
}
