using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Options
{
    internal class BackblazeOptions
    {
        public const string Backblaze = "Backblaze";

        [Required]
        public string BucketId { get; set; }
    }
}
