using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Infrastructure.Data;
using MixedDreams.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Services
{
    internal class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IUnitOfWork> _unitOfWork;

        public RepositoryManager(AppDbContext context)
        {
            _unitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
        }

        public IUnitOfWork UnitOfWork => _unitOfWork.Value;
    }
}
