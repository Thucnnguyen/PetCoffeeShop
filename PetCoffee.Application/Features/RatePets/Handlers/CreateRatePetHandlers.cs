
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.RatePets.Commands;
using PetCoffee.Application.Features.RatePets.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.RatePets.Handlers;

public class CreateRatePetHandlers : IRequestHandler<CreatePetRateCommand, RatePetResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;
	private readonly ICacheService _cacheService;

	public CreateRatePetHandlers(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper, ICacheService cacheService)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
		_cacheService = cacheService;
	}

	public async Task<RatePetResponse> Handle(CreatePetRateCommand request, CancellationToken cancellationToken)
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

		var pet = await _unitOfWork.PetRepository
						.Get(p => p.Id == request.PetId && !p.Deleted)
						.FirstOrDefaultAsync();

		if (pet == null) 
		{
			throw new ApiException(ResponseCode.PetNotExisted);
		}

		var existedRatePet = await _unitOfWork.RatePetRespository
							.Get(rp => rp.PetId == request.PetId && rp.CreatedById == currentAccount.Id)
							.Include(rp => rp.CreatedBy)
							.FirstOrDefaultAsync();

		if (existedRatePet != null)
		{
			existedRatePet.Comment = request.Comment;
			existedRatePet.Rate = request.Rate;
			await _unitOfWork.RatePetRespository .UpdateAsync(existedRatePet);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<RatePetResponse>(existedRatePet);
		}

		var newRatePet = new RatePet()
		{
			PetId = request.PetId,
			Comment = request.Comment,
			Rate = request.Rate,
		};

		await _unitOfWork.RatePetRespository.AddAsync(newRatePet);
		await _unitOfWork.SaveChangesAsync();

		newRatePet.CreatedBy = currentAccount;
		await _cacheService.RemoveAsync(pet.Id.ToString());
		return _mapper.Map<RatePetResponse>(newRatePet);
	}
}
