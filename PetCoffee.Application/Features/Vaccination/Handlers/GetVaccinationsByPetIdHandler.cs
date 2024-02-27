using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Vaccination.Models;
using PetCoffee.Application.Features.Vaccination.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.Linq.Expressions;

namespace PetCoffee.Application.Features.Vaccination.Handlers
{
    public class GetVaccinationsByPetIdHandler : IRequestHandler<GetVaccinationsByPetIdQuery, IList<VaccinationResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IMapper _mapper;

        public GetVaccinationsByPetIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IList<VaccinationResponse>> Handle(GetVaccinationsByPetIdQuery request, CancellationToken cancellationToken)
        {
       

            var Pet = (await _unitOfWork.PetRepository.GetAsync(
                    predicate: p => p.Id == request.PetId && p.PetStatus == PetStatus.Active,
                    //includes: new List<Expression<Func<Domain.Entities.Vaccination, object>>>()
                    //    {
                    //shop => shop.CreatedBy
                    //    },
                    disableTracking: true
                    )).FirstOrDefault();

            if (Pet == null)
            {
                throw new ApiException(ResponseCode.PetNotExisted);
            }

            var Vaccinations = await _unitOfWork.VaccinationRepository.GetAsync(p => p.PetId == request.PetId);
            var response = Vaccinations.Select(p => _mapper.Map<VaccinationResponse>(p)).ToList();
            return response;
        }
    }
}
