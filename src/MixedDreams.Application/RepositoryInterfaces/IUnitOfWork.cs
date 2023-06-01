using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.RepositoryInterfaces
{
    public interface IUnitOfWork
    {
        public ICompanyRepository CompanyRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IIngredientRepository IngredientRepository { get; }
        public IProductRepository ProductRepository { get; }
        public IBusinessLocationRepository BusinessLocationRepository { get; }
        public IProductCategoryRepository ProductCategoryRepository { get; }
        public IProductHistoryRepository ProductHistoryRepository { get; }
        public IDeviceRepository DeviceRepository { get; }
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
