using Microsoft.AspNetCore.Mvc.ModelBinding;
using MixedDreams.Domain.Entities;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.ProductFeatures.ProductIngredient
{
    public sealed record PostProductIngredientDto
    {
        [BindRequired]
        public Guid Id { get; init; }
        [BindRequired]
        public bool HasAmount { get; init; }
        public float? Amount { get; init; }
        public Unit? Unit { get; init; }
    }
}
