using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PostCategory.Commands;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Features.Vaccination.Commands;
using PetCoffee.Application.Features.Vaccination.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TmsApi.Common;

namespace PetCoffee.Application.Features.Vaccination.Handlers
{
    public class UpdateVaccinationHandler : IRequestHandler<UpdateVaccinationCommand, VaccinationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;
        public UpdateVaccinationHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService; 
        }
        public async Task<VaccinationResponse> Handle(UpdateVaccinationCommand request, CancellationToken cancellationToken)
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
            var ExistedVaccination = await _unitOfWork.VaccinationRepository.GetByIdAsync(request.Id);
            if (ExistedVaccination == null)
            {
                throw new ApiException(ResponseCode.VaccinationNotExisted);
            }
            Assign.Partial(request, ExistedVaccination);

            await _unitOfWork.VaccinationRepository.UpdateAsync(ExistedVaccination);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<VaccinationResponse>(ExistedVaccination);
        }
    }
}
