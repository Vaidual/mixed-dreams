using MixedDreams.Core.Repositories;
using MixedDreams.Core.RepositoryInterfaces;
using MixedDreams.Core.ServicesInterfaces;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Core.Services
{
    public class RepositoryManager : IRepositoryManager
    {
        private Lazy<IUnitOfWork> _unitOfWork;

        public RepositoryManager(AppDbContext context)
        {
            _unitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(context));
        }

        public IUnitOfWork UnitOfWork => _unitOfWork.Value;
    }
}
