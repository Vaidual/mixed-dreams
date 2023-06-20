using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class ProductPreparation : BaseEntity
    {
        public OrderProduct OrderProduct { get; set; }
        public Guid OrderProductId { get; set; }

        public Cook? Cook { get; set; }

        public ProductPreparation? NextProductInQueue { get; set; }
        public Guid? NextProductInQueueId { get; set; }
    }
}
