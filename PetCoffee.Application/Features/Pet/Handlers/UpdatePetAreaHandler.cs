using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Pet.Handlers;

public class UpdatePetAreaHandler : IRequestHandler<UpdatePetAreaCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;
	private readonly ICacheService _cacheService;

	public UpdatePetAreaHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper, ICacheService cacheService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
		_cacheService = cacheService;
	}

	public async Task<bool> Handle(UpdatePetAreaCommand request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		var area = await _unitOfWork.AreaRepsitory.GetAsync(a => !a.Deleted && a.Id == request.AreaId);
		if (!area.Any())
		{
			throw new ApiException(ResponseCode.AreaNotExist);
		}

		var listPets = await _unitOfWork.PetRepository.Get(e => !e.Deleted && e.PetAreas.Any(pa => pa.AreaId == request.AreaId && pa.EndTime == null)).ToListAsync();
		if (listPets == null)
		{
			return false;
		}

		foreach (var pet in listPets)
		{
			if (!request.PetIds.Contains(pet.Id))
			{
				var currentPetArea = await _unitOfWork.PetAreaRespository
								.Get(pa => pa.PetId == pet.Id && pa.EndTime == null)
								.FirstOrDefaultAsync();
				if (currentPetArea != null)
				{
					currentPetArea.EndTime = DateTime.UtcNow;
					await _unitOfWork.PetAreaRespository.UpdateAsync(currentPetArea);
					await _cacheService.RemoveAsync(pet.Id.ToString(), cancellationToken);
				}
				request.PetIds.Remove(pet.Id);
			}
		}
		foreach (var petId in request.PetIds)
		{
			var checkPet = await _unitOfWork.PetRepository.GetAsync(p => p.Id == petId && !p.Deleted);
			if (!checkPet.Any())
			{
				continue;
			}
			var currentPetArea = await _unitOfWork.PetAreaRespository
								.Get(pa => pa.PetId == petId && pa.EndTime == null)
								.FirstOrDefaultAsync();
			if (currentPetArea != null)
			{
				currentPetArea.EndTime = DateTime.UtcNow;
				await _unitOfWork.PetAreaRespository.UpdateAsync(currentPetArea);
				await _cacheService.RemoveAsync(petId.ToString(), cancellationToken);
			}
			await _unitOfWork.PetAreaRespository.AddAsync(new PetArea()
			{
				AreaId = request.AreaId,
				StartTime = DateTimeOffset.UtcNow,
				PetId = petId
			});
		}


		await _unitOfWork.SaveChangesAsync();

		return true;
	}
}
