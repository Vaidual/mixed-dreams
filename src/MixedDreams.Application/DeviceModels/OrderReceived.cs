using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.DeviceModels
{
    public class OrderReceived
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
    }
}
