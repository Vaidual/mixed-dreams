using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.OrderFeatures.CreatePaymentIntent
{
    public class CreatePaymentIntentResponse
    {
        public string ClientSecret { get; set; }
        public string PublishableKey { get; set; }
        public string EphemeralKey { get; set; }
        public string CustomerId { get; set; }
    }
}
