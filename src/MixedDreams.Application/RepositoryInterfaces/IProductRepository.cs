using MixedDreams.Infrastructure.DeviceModels;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetProduct;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetProductWithDetails;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MixedDreams.Application.Features.ProductFeatures.Dto;

namespace MixedDreams.Infrastructure.RepositoryInterfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        public Task<GetProductResponse?> GetProduct(Guid id, CancellationToken cancellationToken = default);
        public void CreateProductHistory(Product product);
        public Task<bool> IsNameTaken(string name);
        public void ClearProductIngredients(Guid productId);
        public Task<List<Product>> GetProductNamesAsync(string key, int count, CancellationToken cancellationToken = default);
        public Task<ProductPages> GetPages(CancellationToken cancellationToken, int page = 0, int size = 20, string? key = "", string? category = null, string? sort = null);
        public Task<GetProductWithDetailsResponse?> GetProductInformation(Guid id, CancellationToken cancellationToken = default);
        public Task<string> GenerateUniqueCopyName(string originalName);
        public Task<int> CountImageOccurrencesAsync(string link);
    }
}
