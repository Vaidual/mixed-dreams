using Bytewizer.Backblaze.Extensions;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Services
{
    internal class CooksService : ICooksService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CooksService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateCooksNumber(Guid companyId, short newCooksNumber)
        {
            int cooksNumber = await _unitOfWork.CookRepository.CountAsync();
            if (cooksNumber < newCooksNumber)
            {
                List<Cook> newCooks = new();
                for (int i = 0; i < newCooksNumber - cooksNumber; i++)
                {
                    newCooks.Add(new Cook
                    {
                        CompanyId = companyId,
                    });
                }
                _unitOfWork.CookRepository.AddRange(newCooks);
            }
            else if (cooksNumber > newCooksNumber)
            {
                _unitOfWork.CookRepository.RemoveRange(await _unitOfWork.CookRepository.GetAsync(cooksNumber - newCooksNumber));
            }
        }
    }
}
