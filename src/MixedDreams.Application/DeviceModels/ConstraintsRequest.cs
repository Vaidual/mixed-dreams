using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.DeviceModels
{
    public class ConstraintsRequest
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }
    }
}
