using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Application.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
    public class GetReservationHandler : IRequestHandler<GetReservationQuery, ReservationDetailResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public GetReservationHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ReservationDetailResponse> Handle(GetReservationQuery request, CancellationToken cancellationToken)
        {
            //          var reservation = await _unitOfWork.ReservationRepository.GetByIdAsync(request.Id);
            //          if (reservation == null)
            //          {
            //              throw new ApiException(ResponseCode.ReservationNotExist);
            //          }



            //          var orderDetail = await _unitOfWork.TransactionRepository.Get(
            //    predicate: detail => detail.Id == request.DetailId && detail.ReservationId == request.Id,
            //    includes: new List<Expression<Func<OrderDetail, object>>>()
            //    {
            //              detail => detail.Service
            //    }
            //).FirstOrDefaultAsync(cancellationToken);

            var orderQuery = await _unitOfWork.ReservationRepository.GetAsync(
         predicate: order => order.Id == request.Id,
         disableTracking: true);
            var order = await orderQuery
                //.Include(order => order.Locker)
                //.Include(order => order.SendBox)
                //.Include(order => order.ReceiveBox)
                //.Include(order => order.Sender)
                //.Include(order => order.Receiver)
                //.Include(order => order.Staff)
                .Include(order => order.Transactions).FirstOrDefaultAsync(cancellationToken);
            //.ThenInclude(detail => detail.Service)
            //.Include(order => order.Details)
            //.ThenInclude(detail => detail.Items)
            if (order == null)
            {
                throw new ApiException(ResponseCode.ReservationNotExist);
            }

            //order.Timelines = order.Timelines.OrderByDescending(timeline => timeline.CreatedAt).ToList();

            return _mapper.Map<ReservationDetailResponse>(order);


            return _mapper.Map<ReservationDetailResponse>(order);



        }
    }
}
