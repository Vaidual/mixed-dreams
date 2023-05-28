using Bytewizer.Backblaze.Client;
using Bytewizer.Backblaze.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Extensions.Msal;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Application.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace MixedDreams.Application.Services
{
    internal class BackblazeService : IBackblazeService
    {
        const string FilePathBase = "https://f005.backblazeb2.com/file/MixedDreams/";
        private readonly IStorageClient _client;
        private readonly BackblazeOptions _backblazeOptions;

        public BackblazeService(IStorageClient client, IOptions<BackblazeOptions> backblazeOptions)
        {
            _client = client;
            _backblazeOptions = backblazeOptions.Value;
        }

        public async Task<BackblazeFile> UploadImage(IFormFile image)
        {
            var imageId = Guid.NewGuid() + ".jpg";
            var result = await _client.UploadAsync(_backblazeOptions.BucketId, imageId, image.OpenReadStream());
            if (!result.IsSuccessStatusCode) throw new AggregateException("Cannot upload image to backblaze storage.", new Exception(result.Error.Message));
            return new BackblazeFile
            {
                Path = FilePathBase + result.Response.FileName,
                Id = result.Response.FileId,
                FileName = result.Response.FileName
            };
        }

        public async Task DeleteImage(string field, string fileName)
        {
            var result = await _client.Files.DeleteAsync(field, fileName);
            if (!result.IsSuccessStatusCode) throw new AggregateException("Cannot delete image from backblaze storage.", new Exception(result.Error.Message));
        }
    }
}
