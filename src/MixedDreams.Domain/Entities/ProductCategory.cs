using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class ProductCategory : BaseEntity, IHaveSoftDelete
    {
        public string Name { get; set; }

        public List<Product> Products { get; set; }
        public DateTimeOffset? DateDeleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
