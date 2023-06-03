using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.DeviceModels
{
    public class LowWaterNotify : IDeviceMessage
    {
        public string DeviceId { get; set; }
        public float WaterLevel { get; set; }
    }
}
