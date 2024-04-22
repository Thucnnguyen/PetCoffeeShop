using AutoMapper;
using LinqKit;
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
	public class GetAllReservationByAccountHandler : IRequestHandler<GetAllReservationByAccountQuery, PaginationResponse<PetCoffee.Domain.Entities.Reservation, ReservationResponse>>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly ICurrentAccountService _currentAccountService;
		public GetAllReservationByAccountHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_currentAccountService = currentAccountService;
		}
		public async Task<PaginationResponse<Domain.Entities.Reservation, ReservationResponse>> Handle(GetAllReservationByAccountQuery request, CancellationToken cancellationToken)
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
			var reservations = new List<Domain.Entities.Reservation>();
			if (currentAccount.IsCustomer)
			{
				var expression = request.GetExpressions().And(order => order.CreatedById == currentAccount.Id);
				reservations = await _unitOfWork.ReservationRepository.Get(
							predicate: expression,
							includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Reservation, object>>>
							{
								 o => o.CreatedBy,
								 o => o.Area,
								 o => o.Area.PetCoffeeShop,
								 o => o.Transactions
							},
							disableTracking: true)
						   .OrderByDescending(o => o.Id)

						   .ToListAsync();
			}
			if (currentAccount.IsStaff || currentAccount.IsManager)
			{
				var expression = request.GetExpressions();

				if (request.ShopId != null)
				{
					expression = expression.And(order => order.Area.PetcoffeeShopId == request.ShopId);
				}
				else
				{
					expression = expression.And(order => currentAccount.AccountShops.Select(acs => acs.ShopId).Contains(order.Area.PetcoffeeShopId));
				}
				reservations = await _unitOfWork.ReservationRepository.Get(
							 predicate: expression,
							 includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Reservation, object>>>
							 {
								 o => o.CreatedBy,
								 o => o.Area,
								 o => o.Area.PetCoffeeShop,
								 o => o.Transactions
							 },
							 disableTracking: true)
							.OrderByDescending(o => o.Id)
							.ToListAsync();
			}


			if (reservations == null)
			{

				throw new ApiException(ResponseCode.ReservationNotExist);
			}

			var reservationResponse = reservations
							.Skip((request.PageNumber - 1) * request.PageSize)
							.Take(request.PageSize);


			var response = new List<ReservationResponse>();
			foreach (var reservation in reservationResponse)
			{

				var petCoffeeShopResponse = _mapper.Map<PetCoffeeShopResponse>(reservation.Area.PetCoffeeShop);
				var areaResponse = _mapper.Map<AreaResponse>(reservation.Area);
				var reservationRes = _mapper.Map<ReservationResponse>(reservation);



				var products = await _unitOfWork.ReservationProductRepository
				.Get(rp => rp.ReservationId == reservation.Id)
				.Include(rp => rp.Product).ToListAsync();

				reservationRes.AccountForReservation = _mapper.Map<AccountForReservation>(reservation.CreatedBy);
				reservationRes.Products = products.Select(p => _mapper.Map<ProductForReservationResponse>(p)).ToList();
				reservationRes.PetCoffeeShopResponse = petCoffeeShopResponse;
				reservationRes.AreaResponse = areaResponse;
				if (reservation.Status == OrderStatus.Returned)
				{
					var returnTransaction = reservation.Transactions.Where(t => t.TransactionType == TransactionType.Refund);
					if (returnTransaction != null)
					{
						reservationRes.AmountRefund = returnTransaction.Sum(rt => rt.Amount);
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

