using Bytewizer.Backblaze.Client;
using Bytewizer.Backblaze.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace MixedDreams.Infrastructure.Services
{
    internal class BackblazeService : IBackblazeService
    {
        private readonly IStorageClient _storage;
        private readonly BackblazeOptions _backblazeOptions;

        public BackblazeService(IStorageClient storage, IOptions<BackblazeOptions> backblazeOptions)
        {
            _storage = storage;
            _backblazeOptions = backblazeOptions.Value;
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            await _storage.UploadAsync(_backblazeOptions.BucketId, image.FileName, image.OpenReadStream());
            return $"https://f005.backblazeb2.com/file/MixedDreams/{image.FileName}";
        }
    }
}
