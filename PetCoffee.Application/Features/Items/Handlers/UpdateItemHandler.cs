
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Items.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using TmsApi.Common;

namespace PetCoffee.Application.Features.Items.Handlers;

public class UpdateItemHandler : IRequestHandler<UpdateItemCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentAccountService _currentAccountService;
	private readonly IAzureService _azureService;
	private readonly IMapper _mapper;

	public UpdateItemHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IAzureService azureService, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_currentAccountService = currentAccountService;
		_azureService = azureService;
		_mapper = mapper;
	}

	public async Task<bool> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
	{
		var item = await _unitOfWork.ItemRepository.Get(s => s.ItemId == request.Id && !s.Deleted).FirstOrDefaultAsync();
		if(item == null)
		{
			throw new ApiException(ResponseCode.ItemNotExist);
		}

		Assign.Partial<UpdateItemCommand, Item>(request, item);

		if(request.newIconImg != null)
		{
			await _azureService.CreateBlob(request.newIconImg.FileName, request.newIconImg);
			item.Icon = await _azureService.GetBlob(request.newIconImg.FileName);
		}

		await _unitOfWork.ItemRepository.UpdateAsync(item);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
