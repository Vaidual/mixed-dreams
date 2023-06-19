using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.CompanyFeatures.GetSettings
{
    public class GetCompanySettings
    {
        public required short? CooksNumber { get; set; }
        public required short? MaxSimultaneousOrders { get; set; }
        public required bool AcceptOrders { get; set; }
    }
}
