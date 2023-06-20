using Microsoft.EntityFrameworkCore;
using MixedDreams.Application.Data;
using MixedDreams.Application.Repositories;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Repositories
{
    internal class ProductPreparationRepository : BaseRepository<ProductPreparation>, IProductPreparationRepository
    {
        public ProductPreparationRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<int> GetCountByCompanyAsync(Guid companyId)
        {
            return await Context.ProductPreparations.Include(x => x.Cook).Where(x => x.Cook != null && x.Cook.CompanyId == companyId).CountAsync();
        }
    }
}
