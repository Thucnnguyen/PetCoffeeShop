

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Areas.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;


namespace PetCoffee.Application.Features.Areas.Handlers;

public class GetAreaByIdHandler : IRequestHandler<GetAreaByIdQuery, AreaResponse>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IMapper _mapper;

	public GetAreaByIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_mapper = mapper;
	}

	public async Task<AreaResponse> Handle(GetAreaByIdQuery request, CancellationToken cancellationToken)
	{
		var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
		if (currentAccount == null)
		{
			throw new ApiException(ResponseCode.AccountNotExist);
		}
		if (currentAccount.IsVerify)
		{
			throw new ApiException(ResponseCode.AccountNotActived);
		}

		var area = (await _unitOfWork.AreaRepsitory.GetAsync(a => !a.Deleted && a.Id == request.AreaId)).FirstOrDefault();

		if (area == null)
		{
			throw new ApiException(ResponseCode.AreaNotExist);
		}

		var pets = await _unitOfWork.PetRepository
					.Get(p => p.PetAreas.Any(pa => pa.AreaId == request.AreaId && pa.EndTime == null) && !p.Deleted
							&& p.Name != null && p.Name.ToLower().Contains(request.Search != null ? request.Search : ""))
					.Include(p => p.PetAreas)
					.ThenInclude(pa => pa.Area)
					.Select(p => _mapper.Map<PetResponseForArea>(p))
					.ToListAsync();

		var response = _mapper.Map<AreaResponse>(area);
		response.Pets = pets;
		return response;
	}
}
