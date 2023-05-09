using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private ICompanyRepository _companyRepository;
        private IOrderRepository _orderRepository;
        private ICustomerRepository _customerRepository;
        private IIngredientRepository _ingredientRepository;
        private IProductRepository _productRepository;
        private IBusinessLocationRepository _businessLocationRepository;
        private IProductCategoryRepository _productCategoryRepository;

        public UnitOfWork(AppDbContext context)
        {
            this._context = context;
        }

        public IProductCategoryRepository ProductCategoryRepository => _productCategoryRepository ??= new ProductCategoryRepository(_context);
        public ICompanyRepository CompanyRepository => _companyRepository ??= new CompanyRepository(_context);
        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_context);
        public IIngredientRepository IngredientRepository => _ingredientRepository ??= new IngredientRepository(_context);
        public ICustomerRepository CustomerRepository => _customerRepository ??= new CustomerRepository(_context);
        public IProductRepository ProductRepository => _productRepository ??= new ProductRepository(_context);
        public IBusinessLocationRepository BusinessLocationRepository => _businessLocationRepository ??= new BusinessLocationRepository(_context);

        public Task Save(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
