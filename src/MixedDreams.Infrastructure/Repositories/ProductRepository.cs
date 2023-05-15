using Microsoft.EntityFrameworkCore;
using MixedDreams.Application.Features.ProductFeatures.GetProduct;
using MixedDreams.Application.Features.ProductFeatures.ProductIngredient;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
    }
}
