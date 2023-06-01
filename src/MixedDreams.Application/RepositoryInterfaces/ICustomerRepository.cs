using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.RepositoryInterfaces
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        public Task<Guid?> GetCustomerIdByUserIdAsync(string userId);
    }
}
