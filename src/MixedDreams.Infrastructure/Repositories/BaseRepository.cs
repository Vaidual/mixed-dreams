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

        public BaseRepository(AppDbContext context)
        {
            Context = context;
        }

        public void Create(T entity)
        {
            entity.DateCreated = DateTimeOffset.UtcNow;
            Context.AddAsync(entity);
        }

        public void Update(T entity)
        {
            entity.DateUpdated = DateTimeOffset.UtcNow;
            Context.Update(entity);
        }

        public void Delete(T entity)
        {
            if (entity is SoftDeletableEntity softDeletableEntity)
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
            return Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public Task<List<T>> GetAll(CancellationToken cancellationToken)
        {
            return Context.Set<T>().ToListAsync(cancellationToken);
        }

        public Task<T?> GetByCondition(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            return Context.Set<T>().FirstOrDefaultAsync(expression, cancellationToken);
        }

        public Task<List<T>> GetAllByCondition(Expression<Func<T, bool>> expression, CancellationToken cancellationToken)
        {
            return Context.Set<T>().Where(expression).ToListAsync(cancellationToken);
        }

        public Task<List<T>> GetPagedData(int page, int size, CancellationToken cancellationToken)
        {
            return Context.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
