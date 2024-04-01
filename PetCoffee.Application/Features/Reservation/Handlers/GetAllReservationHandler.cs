using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;

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

            var reservations = await _unitOfWork.ReservationRepository.Get(
						predicate: request.GetExpressions(),
						includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Reservation, object>>>
							 {
								 o => o.CreatedBy,
								 o => o.Area,
								 o => o.Area.PetCoffeeShop,
								 o => o.Transactions
							 },
						disableTracking: true
						).Skip((request.PageNumber - 1) * request.PageSize)
						 .Take(request.PageSize)
						 .ToListAsync(); 
             


            var response = new List<ReservationResponse>();
			foreach (var reservation in reservations)
			{
				var reservationRes = _mapper.Map<ReservationResponse>(reservation);
				var petCoffeeShopResponse = _mapper.Map<PetCoffeeShopResponse>(reservation.Area.PetCoffeeShop);
				var areaResponse = _mapper.Map<AreaResponse>(reservation.Area);

				var products = await _unitOfWork.ReservationProductRepository
				.Get(rp => rp.ReservationId == reservation.Id)
				.Include(rp => rp.Product).ToListAsync();

				reservationRes.AccountForReservation = _mapper.Map<AccountForReservation>(reservation.CreatedBy);
				reservationRes.Products = products.Select(p => _mapper.Map<ProductForReservationResponse>(p)).ToList();
				reservationRes.PetCoffeeShopResponse = petCoffeeShopResponse;
				reservationRes.AreaResponse = areaResponse;
				if (reservation.Status == OrderStatus.Returned) 
				{
					var returnTransaction = reservation.Transactions.FirstOrDefault(t => t.TransactionType == TransactionType.Refund);
					if(returnTransaction != null)
					{
						reservationRes.AmountRefund = returnTransaction.Amount;

					}
				}
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

