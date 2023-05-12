using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Repositories
{
    internal class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public Guid? GetLastProductHistoryId(Guid productId)
        {
            return Context.ProductHistory.Where(x => x.ProductId == productId).MaxBy(x => x.Date)?.Id;
        }
    }
}
