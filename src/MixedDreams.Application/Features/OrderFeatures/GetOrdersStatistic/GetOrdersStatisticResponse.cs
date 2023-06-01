using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.OrderFeatures.GetOrdersStatistic
{
    public class GetOrdersStatisticResponse
    {
        public DateTimeOffset DateTime { get; set; }
        public decimal Income { get; set; }
        public int OrdersAmount { get; set; }
    }
}
