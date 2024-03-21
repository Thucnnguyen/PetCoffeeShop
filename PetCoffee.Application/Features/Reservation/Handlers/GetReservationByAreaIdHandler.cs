using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
    public class GetReservationByAreaIdHandler : IRequestHandler<GetReservationByAreaIdQuery, IList<ReservationResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;
        public GetReservationByAreaIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;
        }
        public async Task<IList<ReservationResponse>> Handle(GetReservationByAreaIdQuery request, CancellationToken cancellationToken)
        {
            //get Current account 
            var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
            if (currentAccount == null)
            {
                throw new ApiException(ResponseCode.AccountNotExist);
            }

            var Posts = _unitOfWork.ReservationRepository.Get(p => p.Area.Id == request.AreaId);

            if (Posts == null)
            {
                return new List<ReservationResponse>();
            }
            var response = Posts.Select(post => _mapper.Map<ReservationResponse>(post)).ToList();


            return response;
        }
    }
}
