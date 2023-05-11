using Microsoft.EntityFrameworkCore;
using MixedDreams.Application.Features.ProductFeatures.GetProduct;
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

        public override Task<List<Product>> GetAll(CancellationToken cancellationToken)
        {
            return Table.AsNoTracking()
                .Include(x => x.Image)
                .ToListAsync(cancellationToken);
        }
    }
}
