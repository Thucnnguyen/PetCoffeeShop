using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Reservation.Commands;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
    public class ConfirmOrderHandler : IRequestHandler<ConfirmOrderCommand, ReservationResponse>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;

        public ConfirmOrderHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;
        }


        public async Task<ReservationResponse> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
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
            if (currentAccount.Role.Equals(Role.Customer))
            {
                throw new ApiException(ResponseCode.Forbidden);
            }

            var order = (await _unitOfWork.ReservationRepository.GetAsync(o => o.Id == request.OrderId)).FirstOrDefault();

            if (order == null)
            {
                throw new ApiException(ResponseCode.ReservationNotExist);
            }

            order.Status = OrderStatus.Success;
            order.UpdatedAt = DateTime.Now;

            await _unitOfWork.ReservationRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            // update area information

            var area = await _unitOfWork.AreaRepsitory.GetByIdAsync(order.AreaId);
            if (area == null)
            {
                throw new ApiException(ResponseCode.AreaNotExist);
            }
            await _unitOfWork.AreaRepsitory.UpdateAsync(area);




            return _mapper.Map<ReservationResponse>(order);
        }
    }
}
