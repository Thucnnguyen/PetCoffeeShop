
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Packages.Commands;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.Packages.Handlers;

public class DeletePackageHandler : IRequestHandler<DeletePackageCommand, bool>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeletePackageHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<bool> Handle(DeletePackageCommand request, CancellationToken cancellationToken)
	{
		var deletedPackage = await _unitOfWork.PackagePromotionRespository.GetByIdAsync(request.Id);
		if (deletedPackage == null)
		{
			throw new ApiException(ResponseCode.PackageNotExist);
		}

		await _unitOfWork.PackagePromotionRespository.DeleteAsync(deletedPackage);
		await _unitOfWork.SaveChangesAsync();
		return true;
	}
}
