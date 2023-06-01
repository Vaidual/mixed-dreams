using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Options
{
    internal class HiveMQOptions
    {
        public const string HiveMQ = "HiveMQ";

        [Required]
        public string ClientId { get; set; }

        [Required]
        public string Server { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
