﻿using MixedDreams.Infrastructure.Features.OrderFeatures.GetOrdersStatistic;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.RepositoryInterfaces
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        public Task<List<GetOrdersStatisticResponse>> GetStatistic(DateTimeOffset start, DateTimeOffset end, TimeSpan step, CancellationToken cancellationToken = default);
    }
}
