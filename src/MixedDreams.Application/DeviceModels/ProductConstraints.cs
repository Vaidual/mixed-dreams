using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.DeviceModels
{
    public class ProductConstraints
    {
        public ProductConstraints(float recommendedTemperature, float recommendedHumidity)
        {
            RecommendedTemperature = recommendedTemperature;
            RecommendedHumidity = recommendedHumidity;
        }

        [JsonProperty(PropertyName = "temperature")]
        public float RecommendedTemperature { get; set; }

        [JsonProperty(PropertyName = "humidity")]
        public float RecommendedHumidity { get; set; }
    }
}
