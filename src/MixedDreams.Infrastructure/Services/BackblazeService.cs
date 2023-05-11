using B2Net;
using B2Net.Models;
using Bytewizer.Backblaze.Client;
using Bytewizer.Backblaze.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Extensions.Msal;
using MixedDreams.Application.Exceptions;
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
        private readonly IStorageClient _client;
        private readonly BackblazeOptions _backblazeOptions;

        public BackblazeService(IStorageClient storage, IOptions<BackblazeOptions> backblazeOptions)
        {
            _client = storage;
            _backblazeOptions = backblazeOptions.Value;
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            var imageId = Guid.NewGuid() + ".jpg";
            var result = await _client.UploadAsync(_backblazeOptions.BucketId, imageId, image.OpenReadStream());
            if (!result.IsSuccessStatusCode) throw new AggregateException("Cannot upload image to backblaze storage.", new Exception(result.Error.Message));
            return $"https://f005.backblazeb2.com/file/MixedDreams/{imageId}";
        }

        public async Task DeleteImage(IFormFile image)
        {
            var client = new B2Client();
            var imageId = Guid.NewGuid() + ".jpg";
            var result = await _client.UploadAsync(_backblazeOptions.BucketId, imageId, image.OpenReadStream());
            if (!result.IsSuccessStatusCode) throw new AggregateException("Cannot upload image to backblaze storage.", new Exception(result.Error.Message));
        }
    }
}
