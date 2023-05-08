using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixedDreams.Application.Dto.Auth
{
    public record TokenResponse
    {
        public TokenResponse(string token)
        {
            Token = token;
        }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
