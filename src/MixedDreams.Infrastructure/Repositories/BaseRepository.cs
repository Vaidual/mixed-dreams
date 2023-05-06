using Microsoft.EntityFrameworkCore;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Common;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Repositories
{
    internal abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext Context;
        protected readonly DbSet<T> Table;

        public BaseRepository(AppDbContext context)
        {
            Context = context;
            Table = context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            if (entity is ITrackableEntity trackableEntity)
            {
                trackableEntity.DateCreated = DateTimeOffset.UtcNow;
            }
            return (await Context.AddAsync(entity)).Entity;
        }

        public void Update(T entity)
        {
            if (entity is ITrackableEntity trackableEntity)
            {
                trackableEntity.DateUpdated = DateTimeOffset.UtcNow;
            }
            Context.Update(entity);
        }

        public void Delete(T entity)
        {
            if (entity is IHaveSoftDelete softDeletableEntity)
            {
                softDeletableEntity.IsDeleted = true;
                softDeletableEntity.DateDeleted = DateTimeOffset.UtcNow;
                Context.Update(softDeletableEntity);
            }
            else
            {
                Context.Remove(entity);
            }
        }

        public Task<T?> Get(Guid id, CancellationToken cancellationToken)
        {
            return Table.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<List<T>> GetAll(CancellationToken cancellationToken)
        {
            return Table.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<List<T>> GetAll(CancellationToken cancellationToken, Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<string>? includes = null)
        {
            IQueryable<T> query = Table;

            if (expression != null)
            {
                query.Where(expression);
            }

            if (includes  != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T?> Get(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            return await Table.Where(expression).AsNoTracking().FirstAsync(cancellationToken);
        }

        public Task<List<T>> GetPagedData(int page, int size, CancellationToken cancellationToken)
        {
            return Table.Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
