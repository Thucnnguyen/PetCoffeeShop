using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
	public class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery, ReservationDetailResponse>
	{
		private readonly IUnitOfWork _unitOfWork;

		private readonly IMapper _mapper;

		public GetReservationByIdHandler(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public async Task<ReservationDetailResponse> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
		{


			var order = await _unitOfWork.ReservationRepository.Get(
							 predicate: order => order.Id == request.Id,
							 includes: new List<System.Linq.Expressions.Expression<Func<Domain.Entities.Reservation, object>>>
							 {
								 p => p.CreatedBy,
								 o => o.Area,
		o => o.Area.PetCoffeeShop
							 },
							 disableTracking: true)
							.FirstOrDefaultAsync();


			if (order == null)
			{
				throw new ApiException(ResponseCode.ReservationNotExist);
			}

			var products = await _unitOfWork.ReservationProductRepository
				.Get(rp => rp.ReservationId == request.Id)
				.ToListAsync();

			//var petCoffeeShop = await _unitOfWork.AreaRepsitory.GetAsync(p => p.PetcoffeeShopId == order.Area.PetcoffeeShopId);
			var petCoffeeShopResponse = _mapper.Map<PetCoffeeShopResponse>(order.Area.PetCoffeeShop);
			var areaResponse = _mapper.Map<AreaResponse>(order.Area);

			var response = _mapper.Map<ReservationDetailResponse>(order);
			response.AccountForReservation = _mapper.Map<AccountForReservation>(order.CreatedBy);
			response.Products = products.Select(p => _mapper.Map<ProductForReservationResponse>(p)).ToList();
			response.PetCoffeeShopResponse = petCoffeeShopResponse;
			response.AreaResponse = areaResponse;
			return response;
		}
	}
}
