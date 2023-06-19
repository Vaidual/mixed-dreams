using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.ProductFeatures.GetCompanyProducts
{
    public sealed class CompanyProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int AmountInStock { get; set; }
    }
}
