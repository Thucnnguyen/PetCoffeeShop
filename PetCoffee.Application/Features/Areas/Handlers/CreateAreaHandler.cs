using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Areas.Commands;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;


namespace PetCoffee.Application.Features.Areas.Handlers
{
	public class CreateAreaHandler : IRequestHandler<CreateAreaCommand, AreaResponse>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IAzureService _azureService;
		private readonly ICurrentAccountService _currentAccountService;
		private readonly IMapper _mapper;

		public CreateAreaHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_azureService = azureService;
			_currentAccountService = currentAccountService;
			_mapper = mapper;
		}

		public async Task<AreaResponse> Handle(CreateAreaCommand request, CancellationToken cancellationToken)
		{
			//get Current account 
			var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
			if (currentAccount == null)
			{
				throw new ApiException(ResponseCode.AccountNotExist);
			}
			if (currentAccount.IsVerify)
			{
				throw new ApiException(ResponseCode.AccountNotActived);
			}

			//check cf shop info
			var shop = await _unitOfWork.PetCoffeeShopRepository
				.Get(s => s.Id == request.PetcoffeeShopId && !s.Deleted)
				.FirstOrDefaultAsync();

			if (shop == null)
			{
				throw new ApiException(ResponseCode.ShopNotExisted);
			}
			var IsExistedArea = await _unitOfWork.AreaRepsitory
									.Get(p => p.PetcoffeeShopId == shop.Id && p.Order == request.Order && !p.Deleted)
									.FirstOrDefaultAsync();
			if (IsExistedArea != null)
			{
				throw new ApiException(ResponseCode.AreaIsExist);
			}

			if (request.Order > 1)
			{
				var IsExistedPreviousArea = await _unitOfWork.AreaRepsitory
								   .Get(p => p.PetcoffeeShopId == shop.Id && p.Order == request.Order - 1 && !p.Deleted)
								   .FirstOrDefaultAsync();
				if (IsExistedPreviousArea == null)
				{
					throw new ApiException(ResponseCode.NotHasPreviousArea);
				}
			}

			var newArea = _mapper.Map<Domain.Entities.Area>(request);
			//if (request.Image != null)
			//{
			//	await _azureService.CreateBlob(request.Image.FileName, request.Image);
			//	newArea.Image = await _azureService.GetBlob(request.Image.FileName);
			//}


			//upload Image
			if (request.Image != null)
			{
				newArea.Image = await _azureService.UpdateloadImages(request.Image);
			}


			await _unitOfWork.AreaRepsitory.AddAsync(newArea);
			await _unitOfWork.SaveChangesAsync();

			var response = _mapper.Map<AreaResponse>(newArea);

			return response;
		}
	}
}
