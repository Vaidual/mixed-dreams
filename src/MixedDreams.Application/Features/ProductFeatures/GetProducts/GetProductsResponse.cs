using MixedDreams.Infrastructure.Features.ProductFeatures.GetProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.ProductFeatures.GetProducts
{
    public class GetProductsResponse
    {
        public IReadOnlyList<GetProductResponse> Products { get; set; }
        public int TotalCount { get; set; }
    }
}
