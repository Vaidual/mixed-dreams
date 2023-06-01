using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Options
{
    internal class JwtOptions
    {
        public const string Jwt = "Jwt";

        [Required]
        public string Audience { get; init; }

        [Required]
        public string Issuer { get; init; }

        [Required]
        [MinLength(8)]
        public string SigningKey { get; init; }
    }
}
