using Microsoft.EntityFrameworkCore;
using MixedDreams.Infrastructure.Features.OrderFeatures.GetOrdersStatistic;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MixedDreams.Infrastructure.DeviceModels;
using MixedDreams.Infrastructure.Exceptions.InternalServerError;
using MixedDreams.Infrastructure.Exceptions.NotFound;
using MixedDreams.Domain.Enums;

namespace MixedDreams.Infrastructure.Repositories
{
    internal class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context) { }

        public override Task<Order?> Get(Guid id, CancellationToken cancellationToken = default)
        {
            return Table.Include(x => x.BusinessLocation).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<GetOrdersStatisticResponse>> GetStatistic(DateTimeOffset start, DateTimeOffset end, TimeSpan step, CancellationToken cancellationToken = default)
        {
            var intervals = Enumerable
                .Range(0, (int)(end - start).TotalMilliseconds / (int)step.TotalMilliseconds + 1)
                .Select(i => new 
                { 
                    IntervalStart = start.Add(step.Multiply(i)), 
                    IntervalEnd = start.Add(step.Multiply(i + 1)) 
                });

            var dateIncome = await Table
                .Where(x => x.DateUpdated >= start &&
                       x.DateUpdated <= end &&
                       x.OrderStatus == Domain.Enums.OrderStatus.Completed)
                .Select(x => new { x.Id, DateUpdated = (DateTimeOffset)x.DateUpdated! })
                .Join(
                    Context.OrderProducts,
                    o => o.Id,
                    op => op.OrderId,
                    (o, op) => new { o.DateUpdated, op.Amount, op.ProductHistoryId })
                .Join(
                    Context.ProductHistory,
                    x => x.ProductHistoryId,
                    ph => ph.Id,
                    (x, ph) => new { Income = x.Amount * ph.Price, x.DateUpdated })
                .ToListAsync();

            return intervals
            .Select(i =>
            {
                var enters = dateIncome.Where(x => x.DateUpdated >= i.IntervalStart &&
                        x.DateUpdated <= i.IntervalEnd);
                return new GetOrdersStatisticResponse
                {
                    DateTime = i.IntervalStart,
                    Income = enters.Sum(x => x.Income) ?? 0,
                    OrdersAmount = enters.Count()
                };
            }).ToList();
            //return dateIncome
            //    .GroupBy(x => new
            //    {
            //        IntervalStart = new DateTimeOffset(x.DateUpdated.Year, x.DateUpdated.Month, x.DateUpdated.Day, x.DateUpdated.Hour, 0, 0, x.DateUpdated.Offset),
            //        IntervalEnd = new DateTimeOffset(x.DateUpdated.Year, x.DateUpdated.Month, x.DateUpdated.Day, x.DateUpdated.Hour, 0, 0, x.DateUpdated.Offset).AddHours(1),
            //    })
            //    .Select(g => new GetOrdersStatisticResponse
            //    {
            //        DateTime = g.Key.IntervalStart,
            //        Income = g.Sum(x => x.Income)
            //    })
            //    .OrderBy(x => x.DateTime)
            //    .ToList();
        }

        public async Task<ProductConstraints> GetOrderProductConstraints(Guid orderId, CancellationToken cancellationToken = default)
        {
            var orderProducts = await Context.OrderProducts.Include(x => x.Product).Where(x => x.OrderId == orderId).ToListAsync(cancellationToken);
            ProductConstraints productConstraints = new()
            {
                RecommendedHumidity = orderProducts.Min(x => x.Product.RecommendedHumidity ?? throw new MissingProductConstraintsException(x.Id.ToString())),
                RecommendedTemperature = orderProducts.Min(x => x.Product.RecommendedTemperature ?? throw new MissingProductConstraintsException(x.Id.ToString())),
            };
            return productConstraints;
        }

        public Task SetOrderReceived(Guid orderId)
        {
            return UpdateOrderStatus(orderId, OrderStatus.Completed);
        }

        public Task SetOrderPrepared(Guid orderId)
        {
            return UpdateOrderStatus(orderId, OrderStatus.Ready);
        }

        private async Task UpdateOrderStatus(Guid orderId, OrderStatus newStatus)
        {
            Order order = await Get(orderId) ?? throw new EntityNotFoundException(nameof(Product), orderId.ToString());
            order.OrderStatus = newStatus;
            Update(order);
        }

    }
}
