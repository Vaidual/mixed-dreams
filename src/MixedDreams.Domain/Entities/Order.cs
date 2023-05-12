using MixedDreams.Domain.Common;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class Order : SoftDeletableTrackableEntity, IMustHaveTenant
    {
        public OrderStatus OrderStatus { get; set; } = OrderStatus.Accepted;

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public Guid BusinessLocationId { get; set; }
        public BusinessLocation BusinessLocation { get; set; }

        public List<OrderProduct> OrderProducts { get; set; }

        public Guid TenantId { get; set; }
    }
}
