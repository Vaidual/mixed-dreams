﻿using MixedDreams.Domain.Common;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class Product : BaseEntity, IMustHaveTenant
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int AmountInStock { get; set; }
        public Visibility Visibility { get; set; } = Visibility.Unavaiable;
        public string? PrimaryImage { get; set; }

        public float RecommendedTemperature { get; set; }
        public float RecommendedHumidity { get; set; }

        public Guid ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set;}

        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        public List<OrderProduct> OrderProducts { get; set; }
        public List<ProductIngredient> ProductIngredients { get; set; }
        public List<ProductHistory> ProductHistory { get; set; }

        public Guid TenantId { get; set; }
    }
}
