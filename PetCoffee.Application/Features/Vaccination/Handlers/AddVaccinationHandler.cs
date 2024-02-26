using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;

using PetCoffee.Application.Features.Vaccination.Commands;
using PetCoffee.Application.Features.Vaccination.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Vaccination.Handlers
{
    public class AddVaccinationHandler : IRequestHandler<AddVaccinationCommand, VaccinationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureService _azureService;
        //private readonly IJwtService _jwtService;
        private readonly ICurrentAccountService _currentAccountService;
        private readonly IMapper _mapper;

        public AddVaccinationHandler(IUnitOfWork unitOfWork, IMapper mapper, IAzureService azureService, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            //_jwtService = jwtService;
            _mapper = mapper;
            _azureService = azureService;
            _currentAccountService = currentAccountService; 
        }

        public async Task<VaccinationResponse> Handle(AddVaccinationCommand request, CancellationToken cancellationToken)
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
            var PetQuery = await _unitOfWork.PetRepository.GetAsync(s => s.Id == request.PetId && s.PetStatus == PetStatus.Active);
            var Pet = PetQuery.FirstOrDefault();
            if (Pet == null)
            {
                throw new ApiException(ResponseCode.PetNotExisted);
            }

            var AddVacction = _mapper.Map<Domain.Entities.Vaccination>(request);
            AddVacction.Pet = Pet;
            AddVacction.VaccinationType = request.VaccinationType;

            // update vaccition picture
            //upload avatar
            if (request.PhotoEvidence != null)
            {
                await _azureService.CreateBlob(request.PhotoEvidence.FileName, request.PhotoEvidence);
                AddVacction.PhotoEvidence = await _azureService.GetBlob(request.PhotoEvidence.FileName);
            }
            await _unitOfWork.VaccinationRepository.AddAsync(AddVacction);
            await _unitOfWork.SaveChangesAsync();
            var response = _mapper.Map<VaccinationResponse>(AddVacction);

            return response;
        }
    }
}
