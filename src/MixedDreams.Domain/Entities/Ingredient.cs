using MixedDreams.Domain.Common;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace MixedDreams.Domain.Entities
{
    public class Ingredient : BaseEntity, IMustHaveTenant
    {
        public string Name { get; set; }

        public List<ProductIngredient> ProductIngredients { get; set; }

        public Guid TenantId { get; set; }
    }
}
