using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class Cook : BaseEntity
    {
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        public OrderProduct? CurrentProductOrder { get; set; }
        public Guid? CurrentProductOrderId { get; set; }

        public OrderProduct? LastProductOrder { get; set; }
        public Guid? LastProductOrderId { get; set; }

        //public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? CurrentEndTime { get; set; }
        public DateTimeOffset? LastEndTime { get; set; }
    }
}
