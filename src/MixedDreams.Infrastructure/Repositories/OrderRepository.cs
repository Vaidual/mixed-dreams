using Microsoft.EntityFrameworkCore;
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

        public override Task<Order?> Get(Guid id, CancellationToken cancellationToken = default)
        {
            return Table.Include(x => x.BusinessLocation).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
