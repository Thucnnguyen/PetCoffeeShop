using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
	public class GetAllReservationHandler : IRequestHandler<GetAllReservationQuery, PaginationResponse<PetCoffee.Domain.Entities.Reservation, ReservationResponse>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentAccountService _currentAccountService;
        public GetAllReservationHandler(
		IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;
        }

        public async Task<PaginationResponse<Domain.Entities.Reservation, ReservationResponse>> Handle(GetAllReservationQuery request, CancellationToken cancellationToken)
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
		


            
            //

            var reservations = await _unitOfWork.ReservationRepository.GetAsync(
		predicate: request.GetExpressions(),
		//includes: new List<Expression<Func<Domain.Entities.Reservation, object>>>()
		//{
		//	  shop => shop.CreatedBy
		//},
		disableTracking: true
	);
            var reservationResponse = reservations.
              Skip((request.PageNumber - 1) * request.PageSize)
              .Take(request.PageSize)
              .ToList();


            var response = new List<ReservationResponse>();
			foreach (var reservation in reservationResponse)
			{
				var reservationRes = _mapper.Map<ReservationResponse>(reservation);

				response.Add(reservationRes);
			}	

			return new PaginationResponse<Domain.Entities.Reservation, ReservationResponse>(
		response,
        reservations.Count(),
		request.PageNumber,
		request.PageSize);
		}
	}
}

