using AutoMapper;
using MediatR;
using OpenAI_API.Images;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var reservations = _unitOfWork.ReservationRepository.Get(rs => rs.CreatedById == currentAccount.Id).OrderByDescending(rs => rs.CreatedAt).ToList();

            
            if (reservations == null)
            {

                throw new ApiException(ResponseCode.ReservationNotExist);
            }
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

