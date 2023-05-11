using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.RepositoryInterfaces
{
    public interface IBaseRepository<T> where T : class
    {
        public Task<bool> EntityExists(Guid id, CancellationToken cancellationToken = default);
        public Task<T> CreateAsync(T entity);
        public void Update(T entity);
        public void Delete(T entity);
        public Task ExecuteDeleteAsync(Expression<Func<T, bool>> predicate);
        public Task<T?> Get(Guid id, CancellationToken cancellationToken = default);
        public Task<T?> Get(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
        public Task<List<T>> GetAll(CancellationToken cancellationToken);
        public Task<List<T>> GetAll(CancellationToken cancellationToken, Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, List<string>? includes = null);
        public Task<List<T>> GetPagedData(int page, int size, CancellationToken cancellationToken);
    }
}
