using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
    public class ReturnOrderHandler : IRequestHandler<ReturnOrderCommand, ReservationResponse>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;

        public ReturnOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;
        }

        public async Task<ReservationResponse> Handle(ReturnOrderCommand request, CancellationToken cancellationToken)
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
            
            var reservation = _unitOfWork.ReservationRepository.Get(p => p.Id == request.OrderId && p.CreatedById == currentAccount.Id && !p.Status.Equals(OrderStatus.Reject)).FirstOrDefault();
            if (reservation == null)
            {
                throw new ApiException(ResponseCode.ReservationNotExist);
            }

            reservation.Status = OrderStatus.Returned;


            await _unitOfWork.ReservationRepository.UpdateAsync(reservation);
            await _unitOfWork.SaveChangesAsync();

            // refund money 


            return _mapper.Map<ReservationResponse>(reservation);
        }
    }
}
