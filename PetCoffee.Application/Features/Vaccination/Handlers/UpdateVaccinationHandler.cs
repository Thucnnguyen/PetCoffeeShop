using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Vaccination.Commands;
using PetCoffee.Application.Features.Vaccination.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using TmsApi.Common;

namespace PetCoffee.Application.Features.Vaccination.Handlers
{
	public class UpdateVaccinationHandler : IRequestHandler<UpdateVaccinationCommand, VaccinationResponse>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentAccountService _currentAccountService;
		private readonly IAzureService _azureService;

		public UpdateVaccinationHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService, IAzureService azureService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentAccountService = currentAccountService;
			_azureService = azureService;
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

			if (request.NewPhotoEvidence != null)
			{
				await _azureService.CreateBlob(request.NewPhotoEvidence.FileName, request.NewPhotoEvidence);
				ExistedVaccination.PhotoEvidence = await _azureService.GetBlob(request.NewPhotoEvidence.FileName);
			}
			await _unitOfWork.VaccinationRepository.UpdateAsync(ExistedVaccination);
			await _unitOfWork.SaveChangesAsync();
			return _mapper.Map<VaccinationResponse>(ExistedVaccination);
		}
	}
}
