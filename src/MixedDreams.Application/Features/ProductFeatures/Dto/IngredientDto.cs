using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.ProductFeatures.Dto
{
    public sealed class IngredientDto
    {
        public string Name { get; set; }
        public bool HasAmount { get; set; }
        public float? Amount { get; set; }
        public Unit? Unit { get; set; }
    }
}
