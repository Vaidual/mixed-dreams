using Microsoft.EntityFrameworkCore;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetProduct;
using MixedDreams.Infrastructure.Features.ProductFeatures.ProductIngredient;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetProductWithDetails;
using MixedDreams.Infrastructure.DeviceModels;
using MixedDreams.Infrastructure.Exceptions.NotFound;
using MixedDreams.Infrastructure.Exceptions.InternalServerError;

namespace MixedDreams.Infrastructure.Repositories
{
    internal class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public override Task<Product?> Get(Guid id, CancellationToken cancellationToken = default)
        {
            return Table.Include(x => x.Image)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public Task<GetProductWithDetailsResponse?> GetProductInformation(Guid id, CancellationToken cancellationToken = default)
        {
            var result = Table
                .Include(x => x.ProductIngredients).ThenInclude(x => x.Product)
                .Include(x => x.Image)
                .Select(x => new GetProductWithDetailsResponse
                {
                    Id = x.Id,
                    AmountInStock = x.AmountInStock,
                    Description = x.Description,
                    Name = x.Name,
                    Price = x.Price,
                    PrimaryImage = x.Image.Path,
                    RecommendedHumidity = x.RecommendedHumidity,
                    RecommendedTemperature = x.RecommendedTemperature,
                    Visibility = x.Visibility,
                    Ingredients = x.ProductIngredients.Select(pi => new GetProductIngredientsDto
                    {
                        Id = pi.IngredientId,
                        Amount = pi.Amount,
                        HasAmount = pi.HasAmount,
                        Name = pi.Ingredient.Name,
                        Unit = pi.Unit,
                    }),
                    ProductCategory = x.ProductCategoryId
                })
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return result;
        }


        public override async Task<IReadOnlyList<Product>> GetAll(CancellationToken cancellationToken)
        {
            return await Table.AsNoTracking()
                .Include(x => x.Image)
                .ToListAsync(cancellationToken);
        }

        public Task<bool> IsNameTaken(string name)
        {
            return ExistAnyAsync(x => x.Name == name);
        }

        public override Product Create(Product entity)
        {
            Product product = base.Create(entity);
            CreateProductHistory(product);
            return product;
        }

        public void CreateProductHistory(Product product)
        {
            Context.ProductHistory.Add(new ProductHistory
            {
                Date = DateTimeOffset.Now,
                Name = product.Name,
                Price = product.Price,
                ProductId = product.Id
            });
        }

        public void ClearProductIngredients(Guid productId)
        {
            Context.ProductIngredient.RemoveRange(Context.ProductIngredient.Where(x => x.ProductId == productId));
        }

        public Task<List<Product>> GetProductNamesAsync(string key, int count, CancellationToken cancellationToken = default)
        {
            return Table.Where(x => x.Name.Contains(key)).Take(count).ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Product>> GetPages(CancellationToken cancellationToken, int page = 0, int size = 20, string? key = "", string? category = null)
        {
            IQueryable<Product> query = Table;
            if (key != null)
            {
                query = query.Where(x => x.Name.Contains(key));
            }
            if (category != null)
            {
                category = category.ToLower();
                Guid? categoryId = (await Context.ProductCategories.FirstOrDefaultAsync(x => x.Name.ToLower() == category))?.Id;
                if (categoryId != null)
                {
                    query = query.Where(x => x.ProductCategoryId ==  categoryId);
                }
            }
            query = query.Skip(page * size).Take(size);
            return await query.Include(x => x.Image).AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<ProductConstraints> GetProductConstraints(Guid id, CancellationToken cancellationToken = default)
        {
            Product product = await Table.FindAsync(new object?[] { id }, cancellationToken: cancellationToken) ?? throw new EntityNotFoundException(nameof(Product), id.ToString());
            if (product.RecommendedTemperature == null || product.RecommendedHumidity == null)
            {
                throw new MissingProductConstraintsException(id.ToString());
            }
            return new ProductConstraints((float)product.RecommendedTemperature, (float)product.RecommendedHumidity!);
        }
    }
}
