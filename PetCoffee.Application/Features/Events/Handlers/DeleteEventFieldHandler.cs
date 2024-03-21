

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Events.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Events.Handlers;

public class DeleteEventFieldHandler : IRequestHandler<DeleteEventFieldCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IMapper _mapper;

    public DeleteEventFieldHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
        _mapper = mapper;
    }

    public async Task<bool> Handle(DeleteEventFieldCommand request, CancellationToken cancellationToken)
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
        var eventField = await _unitOfWork.EventFieldRepsitory.GetByIdAsync(request.EventFieldId);
        if (eventField == null)
        {
            throw new ApiException(ResponseCode.EventFieldIsNotExist);
        }

        await _unitOfWork.EventFieldRepsitory.DeleteAsync(eventField);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}
