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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

            // check exist area
            var area = (await _unitOfWork.AreaRepsitory.GetAsync(a => !a.Deleted && a.Id == request.AreaId)).FirstOrDefault();

            if (area == null)
            {
                throw new ApiException(ResponseCode.AreaNotExist);
            }


            var order = new Domain.Entities.Reservation
            {

                Status = OrderStatus.Processing,
                StartTime = request.StartTime,
                EndTime = request.EndTime ??= request.StartTime.AddHours(2),
                Note = request.Note,
                AreaId = request.AreaId,
                TotalPrice = 0, //
                Discount = 0, //
                Deposit = 0, //
                Code = "test", //
                CreatedById = currentAccount.Id,
                CreatedAt = DateTime.Now

            };

            await _unitOfWork.ReservationRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            // minus money in wallet for booking



            return _mapper.Map<ReservationResponse>(order);


        }
    }
}
