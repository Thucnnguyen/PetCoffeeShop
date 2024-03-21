using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Pet.Handlers;

public class UpdatePetAreaHandler : IRequestHandler<UpdatePetAreaCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;

    public UpdatePetAreaHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
    {
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
    }

    public async Task<bool> Handle(UpdatePetAreaCommand request, CancellationToken cancellationToken)
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

        var area = await _unitOfWork.AreaRepsitory.GetAsync(a => !a.Deleted && a.Id == request.AreaId);
        if (!area.Any())
        {
            throw new ApiException(ResponseCode.AreaNotExist);
        }

        var listPets = await _unitOfWork.PetRepository.GetAsync(e => !e.Deleted && request.PetIds.Any(petId => e.Id == petId));
        if (listPets == null)
        {
            return false;
        }

        foreach (var pet in listPets)
        {
            pet.AreaId = request.AreaId;
            await _unitOfWork.PetRepository.UpdateAsync(pet);
        }
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}
