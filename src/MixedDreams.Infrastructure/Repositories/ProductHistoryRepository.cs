using Microsoft.EntityFrameworkCore;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Repositories
{
    internal class ProductHistoryRepository : BaseRepository<ProductHistory>, IProductHistoryRepository
    {
        public ProductHistoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Guid?> GetLastProductHistoryId(Guid productId)
        {
            return (await Context.ProductHistory.Where(x => x.ProductId == productId).OrderByDescending(x => x.Date).FirstOrDefaultAsync())?.Id;
        }
    }
}
