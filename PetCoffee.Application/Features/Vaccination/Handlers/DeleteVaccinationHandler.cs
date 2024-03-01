using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Vaccination.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;


namespace PetCoffee.Application.Features.Vaccination.Handlers
{
    public class DeleteVaccinationHandler : IRequestHandler<DeleteVaccinationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;

        public DeleteVaccinationHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;
        }

        public async Task<bool> Handle(DeleteVaccinationCommand request, CancellationToken cancellationToken)
        {
            var curAccount = await _currentAccountService.GetCurrentAccount();
            if (curAccount == null)
            {
                throw new ApiException(ResponseCode.AccountNotExist);
            }
            if (curAccount.IsVerify)
            {
                throw new ApiException(ResponseCode.AccountNotActived);
            }

            var vaccination = (await _unitOfWork.VaccinationRepository.GetAsync(f => f.Id == request.VaccinationId)).FirstOrDefault();
            if (vaccination == null)
            {
                throw new ApiException(ResponseCode.VaccinationNotExisted);
            }
            await _unitOfWork.VaccinationRepository.DeleteAsync(vaccination);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
