using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Items.Commands;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.Items.Handlers;

public class DeleteItemHandler : IRequestHandler<DeleteItemCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteItemHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
	{
		var item = await _unitOfWork.ItemRepository.Get(s => s.ItemId == request.Id && !s.Deleted).FirstOrDefaultAsync();
		if (item == null)
		{
			throw new ApiException(ResponseCode.ItemNotExist);
		}
		
		item.DeletedAt = DateTime.UtcNow;

		await _unitOfWork.ItemRepository.UpdateAsync(item);
		await _unitOfWork.SaveChangesAsync();
		return true;

	}
}
