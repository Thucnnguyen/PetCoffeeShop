using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Promotion.Models;
using PetCoffee.Application.Features.Promotion.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Promotion.Handlers
{

	public class GetPromotionByIdHandler : IRequestHandler<GetPromotionByIdQuery, PromotionResponse>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentAccountService _currentAccountService;

		public GetPromotionByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentAccountService = currentAccountService;
		}
		public async Task<PromotionResponse> Handle(GetPromotionByIdQuery request, CancellationToken cancellationToken)
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

			var Promotion = await _unitOfWork.PromotionRepository
					.Get(predicate: p => p.Id == request.Id && !p.Deleted)
					.Include(p => p.AccountPromotions)
					.FirstOrDefaultAsync();



			if (Promotion == null)
			{
				throw new ApiException(ResponseCode.PromotionNotExisted);
			}

			var response = _mapper.Map<PromotionResponse>(Promotion);
			return response;
		}
	}
}
