using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Common
{
    public abstract class SoftDeletableTrackableEntity : BaseEntity, IHaveSoftDelete, ITrackableEntity
    {
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset? DateUpdated { get; set; }
        public DateTimeOffset? DateDeleted { get; set ; }
        public bool IsDeleted { get; set; }
    }
}
