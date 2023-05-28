using Microsoft.EntityFrameworkCore;
using MixedDreams.Application.Features.OrderFeatures.GetOrdersStatistic;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Application.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Repositories
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

    }
}
