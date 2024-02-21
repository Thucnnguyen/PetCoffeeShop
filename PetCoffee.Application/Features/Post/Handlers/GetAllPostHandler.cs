using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.PetCfShop.Queries;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Post.Handlers
{
    public class GetAllPostHandler : IRequestHandler<GetAllPostQuery, PaginationResponse<PetCoffee.Domain.Entities.Post, PostResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public GetAllPostHandler(
        IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginationResponse<Domain.Entities.Post, PostResponse>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.PostRepository.GetAsync(
          predicate: request.GetExpressions(),
          //includes: new List<Expression<Func<Domain.Entities.Post, object>>>()
          //{
          //      shop => shop.CreatedBy
          //},
          disableTracking: true
      );

            var response = new List<PostResponse>();
            foreach (var post in posts)
            {
                var postRes = _mapper.Map<PostResponse>(post);
              
                response.Add(postRes);
            }

            return new PaginationResponse<Domain.Entities.Post, PostResponse>(
        response,
        response.Count(),
        request.PageNumber,
        request.PageSize);
        }
    }
}
