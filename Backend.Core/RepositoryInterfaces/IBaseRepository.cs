using MixedDreams.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Core.RepositoryInterfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T?> Get(Guid id, CancellationToken cancellationToken);
        Task<T?> GetByCondition(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
        Task<List<T>> GetAll(CancellationToken cancellationToken);
        public Task<List<T>> GetAllByCondition(Expression<Func<T, bool>> expression, CancellationToken cancellationToken);
    }
}
