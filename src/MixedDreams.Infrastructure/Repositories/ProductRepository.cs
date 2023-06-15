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
using MixedDreams.Application.Features.ProductFeatures.Dto;
using Polly;
using MixedDreams.Domain.Enums;
using MixedDreams.Application.Constants;
using MixedDreams.Application.Exceptions.InternalServerError;

namespace MixedDreams.Infrastructure.Repositories
{
    internal class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<GetProductResponse?> GetProduct(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await Table
                .Include(x => x.Image)
                .Include(x => x.ProductCategory)
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (product == null) return null;

            await Context.Entry(product)
                .Collection(p => p.ProductIngredients)
                .Query()
                .Include(pi => pi.Ingredient)
                .LoadAsync(cancellationToken);

            product.ProductIngredients.OrderByDescending(x => x.HasAmount);

            return new GetProductResponse
            {
                Id = product.Id,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price ?? throw new NullFieldExcetion(nameof(Product.Price), nameof(Product)),
                PrimaryImage = product.Image?.Path,
                Ingredients = product.ProductIngredients.Select(pi => new IngredientDto
                {
                    Amount = pi.Amount,
                    HasAmount = pi.HasAmount,
                    Name = pi.Ingredient.Name,
                    Unit = pi.Unit,
                }),
                Category = product.ProductCategory.Name,
                Company = new CompanyDto
                {
                    Id = product.Company.Id,
                    Name = product.Company.CompanyName
                }
            };
                
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

        public async Task<ProductPages> GetPages(CancellationToken cancellationToken, int page = 0, int size = 20, string? key = "", string? category = null, string? sort = null)
        {
            IQueryable<Product> query = Table
                .Where(x => x.AmountInStock > 0 &&
                       x.Visibility == Visibility.Visible &&
                       x.RecommendedTemperature != null &&
                       x.RecommendedHumidity != null);
            if (key != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(key.ToLower()));
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

            query = sort switch
            {
                ProductSort.Price => query.OrderBy(x => x.Price),
                ProductSort.PriceDesc => query.OrderByDescending(x => x.Price),
                _ => query.OrderByDescending(x => x.DateCreated),
            };

            int totalCount = await query.CountAsync();
            query = query.Skip(page * size).Take(size);
            return new ProductPages()
            {
                Products = await query.Include(x => x.Image).AsNoTracking().ToListAsync(cancellationToken),
                TotalCount = totalCount
            };
        }

        public async Task<string> GenerateUniqueCopyName(string originalName)
        {
            var copyName = $"{originalName}_copy";
            var existingCopyNames = await Table
                .Where(p => EF.Functions.Like(p.Name, $"{copyName}%"))
                .Select(p => p.Name)
                .ToListAsync();

            if (existingCopyNames.Count == 0)
                return $"{copyName}1";

            var maxCopyNumber = existingCopyNames
                .Select(name => int.TryParse(name.Substring(copyName.Length), out var copyNumber) ? copyNumber : 0)
                .Max();

            return $"{copyName}{maxCopyNumber + 1}";
        }

        public async Task<int> CountImageOccurrencesAsync(string link)
        {
            return await Table.Include(x => x.Image).Where(x => x.Image != null && x.Image.Path == link).CountAsync();
        }

    }
}
