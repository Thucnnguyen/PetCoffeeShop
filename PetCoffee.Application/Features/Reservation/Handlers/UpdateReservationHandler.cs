using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.PostCategory.Commands;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
    public class UpdateReservationHandler : IRequestHandler<UpdateReservationCommand, ReservationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;

        public UpdateReservationHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;     
        }

        public async Task<ReservationResponse> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
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
            var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(request.Id);
            if (reservation == null)
            {
                throw new ApiException(ResponseCode.ReservationNotExist);
            }

            reservation.Comment = request.Comment ??= reservation.Comment;
            await _unitOfWork.ReservationRepository.UpdateAsync(reservation);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReservationResponse>(reservation);
        }
    }
}
