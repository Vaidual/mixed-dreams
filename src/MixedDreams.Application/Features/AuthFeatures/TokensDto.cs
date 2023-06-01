using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.AuthFeatures
{
    public sealed record TokensDto
    {
        public TokensDto(string token)
        {
            AccessToken = token;
        }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
    }
}
