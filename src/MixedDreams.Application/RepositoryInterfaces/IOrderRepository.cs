using MixedDreams.Infrastructure.Features.OrderFeatures.GetOrdersStatistic;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MixedDreams.Infrastructure.DeviceModels;
using MixedDreams.Domain.Enums;
using MixedDreams.Infrastructure.Exceptions.NotFound;

namespace MixedDreams.Infrastructure.RepositoryInterfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        public Task<List<GetOrdersStatisticResponse>> GetStatistic(DateTimeOffset start, DateTimeOffset end, TimeSpan step, CancellationToken cancellationToken = default);
        public Task<ProductConstraints> GetOrderProductConstraints(Guid orderId, CancellationToken cancellationToken = default);
        public Task SetOrderReceived(Guid orderId);
        public Task SetOrderPrepared(Guid orderId);
    }
}
