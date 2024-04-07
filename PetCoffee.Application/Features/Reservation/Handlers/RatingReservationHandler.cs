using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.RatePets.Commands;
using PetCoffee.Application.Features.RatePets.Models;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Handlers
{

	public class RatingReservationHandler : IRequestHandler<RatingReservationCommand, ReservationResponse>
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly ICurrentAccountService _currentAccountService;
		private readonly IMapper _mapper;
		private readonly ICacheService _cacheService;

		public RatingReservationHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper, ICacheService cacheService)
		{
			_unitOfWork = unitOfWork;
			_currentAccountService = currentAccountService;
			_mapper = mapper;
			_cacheService = cacheService;
		}

		public async Task<ReservationResponse> Handle(RatingReservationCommand request, CancellationToken cancellationToken)
		{
			var currentAccount = await _currentAccountService.GetCurrentAccount();
			if (currentAccount == null)
			{
				throw new ApiException(ResponseCode.AccountNotExist);
			}
			if (currentAccount.IsVerify)
			{
				throw new ApiException(ResponseCode.AccountNotActived);
			}

			var reservation = await _unitOfWork.ReservationRepository.GetAsync(r => r.Id == request.Id && r.StartTime < DateTimeOffset.Now);
			if (reservation.FirstOrDefault() == null)
			{
				throw new ApiException(ResponseCode.ReservationNotExist);
			}
			var rateReservation = reservation.FirstOrDefault();

			if (rateReservation.CreatedById != currentAccount.Id)
			{
				throw new ApiException(ResponseCode.PermissionDenied);
			}

			rateReservation.Comment = request.Comment;
			rateReservation.Rate = request.Rate;

			await _unitOfWork.ReservationRepository.UpdateAsync(rateReservation);
			await _unitOfWork.SaveChangesAsync();

			return _mapper.Map<ReservationResponse>(rateReservation);
		}
	}
}
