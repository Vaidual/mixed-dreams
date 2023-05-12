using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.RepositoryInterfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        public Guid? GetLastProductHistoryId(Guid productId);
    }
}
