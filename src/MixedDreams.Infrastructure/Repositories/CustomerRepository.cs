using Microsoft.EntityFrameworkCore;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Repositories
{
    internal class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Guid?> GetCustomerIdByUserIdAsync(string userId)
        {
            return (await Table.FirstOrDefaultAsync(x => x.ApplicationUserId == userId))?.Id;
        }
    }
}
