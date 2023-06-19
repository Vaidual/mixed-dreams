using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Options
{
    internal class StripeOptions
    {
        public const string Backblaze = "Stripe";

        [Required]
        public string BucketId { get; set; }
    }
}
