using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Items.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;


namespace PetCoffee.Application.Features.Items.Handlers
{
	public class GetAllItemHandler : IRequestHandler<GetAllItemQuery, PaginationResponse<Item, ItemResponse>>
	{

		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentAccountService _currentAccountService;

		public GetAllItemHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_currentAccountService = currentAccountService;
		}

		public async Task<PaginationResponse<Item, ItemResponse>> Handle(GetAllItemQuery request, CancellationToken cancellationToken)
		{
			var CurrentUser = await _currentAccountService.GetCurrentAccount();
			if (CurrentUser == null)
			{
				throw new ApiException(ResponseCode.AccountNotExist);
			}
			if (CurrentUser.IsVerify)
			{
				throw new ApiException(ResponseCode.AccountNotActived);
			}
			var items = await _unitOfWork.ItemRepository.GetAsync();
			//var Expression = request.GetExpressions();
			//var items = await _unitOfWork.ItemRepository.Get(Expression).ToListAsync();
			var response = new List<ItemResponse>();
			foreach (var i in items)
			{
				var itemResponse = _mapper.Map<ItemResponse>(i);
				response.Add(itemResponse);
			}
			response = response.OrderBy(x => x.Price).ToList();

			return new PaginationResponse<Item, ItemResponse>(
				response,
				response.Count(),
				request.PageNumber,
				request.PageSize);
		}
	}
}
