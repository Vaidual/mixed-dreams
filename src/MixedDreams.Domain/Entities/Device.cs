using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class Device : BaseEntity, IHaveSoftDelete
    {
        public string Identifier { get; set; }

        public BusinessLocation BusinessLocation { get; set; }

        public Company Company { get; set; }
        public Guid? CompanyId { get; set; }

        public DateTimeOffset? DateDeleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
