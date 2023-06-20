using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class Cook : BaseEntity
    {
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        public ProductPreparation CurrentProductPreparation { get; set; }
        public Guid? CurrentProductPreparationId { get; set; }

        public ProductPreparation LastProductPreparation { get; set; }
        public Guid? LastProductPreparationId { get; set; }

        public DateTimeOffset? CurrentEndTime { get; set; }
        public DateTimeOffset? LastEndTime { get; set; }
    }
}
