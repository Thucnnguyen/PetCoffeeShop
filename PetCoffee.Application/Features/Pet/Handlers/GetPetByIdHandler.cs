

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Pet.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Pet.Handlers;

internal class GetPetByIdHandler : IRequestHandler<GetPetByIdQuery, PetResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly ICacheService _cacheService;

	public GetPetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService, ICacheService cacheService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_currentAccountService = currentAccountService;
		_cacheService = cacheService;
	}

	public async Task<PetResponse> Handle(GetPetByIdQuery request, CancellationToken cancellationToken)
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

		var cachePet = await _cacheService.GetAsync<PetResponse>(request.Id.ToString(), cancellationToken);
		if (cachePet != null)
		{
			return cachePet;
		}

		var Pet = (await _unitOfWork.PetRepository
				.GetAsync(
						predicate: p => p.Id == request.Id && !p.Deleted,
						includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Pet, object>>>
						{
							p => p.Area
						})
				).FirstOrDefault();


		if (Pet == null)
		{
			throw new ApiException(ResponseCode.PetNotExisted);
		}

		var response = _mapper.Map<PetResponse>(Pet);

		var TopDonations = _unitOfWork.TransactionRepository
			.Get(tr => tr.PetId == Pet.Id &&
						tr.TransactionStatus == Domain.Enums.TransactionStatus.Done)
			.Include(tr => tr.CreatedBy)
			.GroupBy(tr => tr.CreatedBy)
			.Select(group => new DonationAccount()
			{
				AvatarUrl = group.Key.Avatar,
				Id = group.Key.Id,
				Name = group.Key.FullName,
				TotalDonate = group.Sum(tr => tr.Amount)
			})
			.OrderByDescending(donationAccount => donationAccount.TotalDonate)
			.Take(10)
			.ToList();

		response.Accounts = TopDonations;
		await _cacheService.SetAsync(response.Id.ToString(), response, cancellationToken);
		return response;
	}
}
