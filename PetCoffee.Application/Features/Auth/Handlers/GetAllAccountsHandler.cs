

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Auth.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, PaginationResponse<Account, AccountForRecord>>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public GetAllAccountsHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<PaginationResponse<Account, AccountForRecord>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
	{
		var accounts = await _unitOfWork.AccountRepository
			.GetAsync(request.GetExpressions());

		return new PaginationResponse<Account, AccountForRecord>(
				accounts,
				request.PageNumber,
				request.PageSize,
				s => _mapper.Map<AccountForRecord>(s));
	}
}
