using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Common;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public Task<bool> EntityExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Table.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public virtual T Create(T entity)
        {
            if (entity is ITrackableEntity trackableEntity)
            {
                trackableEntity.DateCreated = DateTimeOffset.UtcNow;
            }
            return Context.Add(entity).Entity;
        }

        public virtual void Update(T entity)
        {
            if (entity is ITrackableEntity trackableEntity)
            {
                trackableEntity.DateUpdated = DateTimeOffset.UtcNow;
            }
            Context.Update(entity);
        }

        public virtual void Delete(T entity)
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

        public virtual async Task<T?> Get(Guid id, CancellationToken cancellationToken = default)
        {
            return await Table.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
        }

        public virtual async Task<IReadOnlyList<T>> GetAll(CancellationToken cancellationToken)
        {
            return await Table.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAll(CancellationToken cancellationToken, Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<string>? includes = null)
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
            return Table.Skip(page * size).Take(size).AsNoTracking().ToListAsync(cancellationToken);
        }

        public Task ExecuteDeleteAsync(Expression<Func<T,bool>> predicate)
        {
            return Table.Where(predicate).ExecuteDeleteAsync();
        }

        public Task ExecuteUpdateAsync(Expression<Func<T, bool>> predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            return Table.Where(predicate).ExecuteUpdateAsync(setPropertyCalls);
        }

        public Task<bool> ExistAnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return Table.AnyAsync(predicate, cancellationToken);
        }
    }
}
