using MixedDreams.Domain.Common;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class Customer : SoftDeletableTrackableEntity
    {
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public List<Order> Orders { get; set; }
    }
}
