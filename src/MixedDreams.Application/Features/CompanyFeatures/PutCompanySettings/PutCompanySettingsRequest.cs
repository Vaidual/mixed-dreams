using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.CompanyFeatures.PutCompanySettings
{
    public class PutCompanySettingsRequest
    {
        public short? CooksNumber { get; set; }
        public short? MaxSimultaneousOrders { get; set; }
        public bool AcceptOrders { get; set; }
    }
}
