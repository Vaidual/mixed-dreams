using Microsoft.AspNetCore.Identity;
using MixedDreams.Domain.Common;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class Company : SoftDeletableTrackableEntity
    {
        public string CompanyName { get; set; }
        public DateTime Birthday { get; set; }
        public Address Address { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public List<Product> Products { get; set; }
        public List<Device> Devices { get; set; }
        public List<BusinessLocation> BusinessLocations { get; set; }
    }
}
