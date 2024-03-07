using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Queries;
using PetCoffee.Application.Features.Reservation.Models;
using PetCoffee.Application.Features.Reservation.Queries;
using PetCoffee.Application.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Reservation.Handlers
{
    public class GetAllReservationHandler : IRequestHandler<GetAllReservationQuery, PaginationResponse<PetCoffee.Domain.Entities.Reservation, ReservationResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetAllReservationHandler(
        IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<Domain.Entities.Reservation, ReservationResponse>> Handle(GetAllReservationQuery request, CancellationToken cancellationToken)
        {
            
            var posts = await _unitOfWork.ReservationRepository.GetAsync(
        predicate: request.GetExpressions(),
        //includes: new List<Expression<Func<Domain.Entities.Post, object>>>()
        //{
        //      shop => shop.CreatedBy
        //},
        disableTracking: true
    );

            var response = new List<ReservationResponse>();
            foreach (var post in posts)
            {
                var postRes = _mapper.Map<ReservationResponse>(post);

                response.Add(postRes);
            }

            return new PaginationResponse<Domain.Entities.Reservation, ReservationResponse>(
        response,
        response.Count(),
        request.PageNumber,
        request.PageSize);
        }
    }
}

