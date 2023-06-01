using MixedDreams.Domain.Common;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class BusinessLocation : BaseEntity, IMustHaveTenant
    {
        public string Name { get; set; }
        public Address Address { get; set; }

        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        public Device Device { get; set; }
        public Guid? DeviceId { get; set; }

        public List<Order> Orders { get; set; }
        public Guid TenantId { get; set; }
    }
}
