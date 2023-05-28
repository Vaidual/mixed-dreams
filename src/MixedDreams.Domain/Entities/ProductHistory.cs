using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class ProductHistory : BaseEntity, IMustHaveTenant
    {
        public DateTimeOffset Date { get; set; }
        public string Name { get; set; }
        public decimal? Price { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid TenantId { get; set; }

        public List<OrderProduct> OrderProducts { get; set; }
    }
}
