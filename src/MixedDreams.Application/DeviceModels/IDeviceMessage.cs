using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.DeviceModels
{
    public interface IDeviceMessage
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }
    }
}
