using MixedDreams.Domain.Common;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class ProductIngredient : BaseEntity, IMustHaveTenant
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        public bool HasAmount { get; set; }
        public float? Amount { get; set; }
        public Unit? Unit { get; set; }

        public Guid TenantId { get; set; }
    }
}
