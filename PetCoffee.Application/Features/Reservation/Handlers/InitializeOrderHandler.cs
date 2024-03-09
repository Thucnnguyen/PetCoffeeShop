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
    public class InitializeOrderHandler : IRequestHandler<InitializeOrderCommand, ReservationResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;

        public InitializeOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;
        }
        public async Task<ReservationResponse> Handle(InitializeOrderCommand request, CancellationToken cancellationToken)
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
            if (!currentAccount.Role.Equals(Role.Customer))
            {
                throw new ApiException(ResponseCode.PermissionDenied);
            }



            return null;


        }
    }
}
