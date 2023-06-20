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
                List<Cook> cooks = await _unitOfWork.CookRepository.GetAllAsync();
                List<Cook> newCooks = new();
                for (int i = 0; i < newCooksNumber - cooksNumber; i++)
                {
                    newCooks.Add(new Cook
                    {
                        CompanyId = companyId,
                    });
                }
                if (await _unitOfWork.ProductPreparationRepository.GetCountByCompanyAsync(companyId) > cooks.Count)
                {
                    await ReorderPreparationsWithMoreCooks(cooks, newCooks);
                }      
                _unitOfWork.CookRepository.AddRange(newCooks);
            }
            else if (cooksNumber > newCooksNumber)
            {
                _unitOfWork.CookRepository.RemoveRange(await _unitOfWork.CookRepository.GetAsync(cooksNumber - newCooksNumber));
            }
        }

        private async Task ReorderPreparationsWithMoreCooks(List<Cook> cooks, List<Cook> newCooks)
        {
            DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;
            Cook cookToAdd = newCooks
                .MinBy(newCook => (newCook.LastEndTime - currentDateTime)?.TotalMinutes ?? 0.0)!;
            double minLastEndTimeDiff = (cookToAdd.LastEndTime - currentDateTime)?.TotalMinutes ?? 0.0;
            await _unitOfWork.CookRepository.LoadCurrentPreparationsNext(cooks);
            Cook? cookToFree = cooks
                .Where(x => x.CurrentProductPreparation.NextProductInQueueId != null)
                .FirstOrDefault(x => (x.LastEndTime - currentDateTime)?.TotalMinutes > minLastEndTimeDiff);
            while (cookToFree != null)
            {
                

                cookToAdd = newCooks
                    .MinBy(newCook => (newCook.LastEndTime - currentDateTime)?.TotalMinutes ?? 0.0)!;
                minLastEndTimeDiff = (cookToAdd.LastEndTime - currentDateTime)?.TotalMinutes ?? 0.0;
                cookToFree = cooks
                    .Where(x => x.CurrentProductPreparation.NextProductInQueueId != null)
                    .FirstOrDefault(x => (x.LastEndTime - currentDateTime)?.TotalMinutes > minLastEndTimeDiff);
            }
        }
    }
}
