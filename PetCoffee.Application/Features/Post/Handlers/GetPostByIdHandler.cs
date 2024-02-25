using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Application.Features.Post.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Enums;


namespace PetCoffee.Application.Features.Post.Handlers
{
    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;

        public GetPostByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;
        }

        public async Task<PostResponse> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {

            var Posts = await _unitOfWork.PostRepository.Get(p => p.Id == request.Id && p.Status == PostStatus.Active)
            .Include(p => p.PostCategories)
            .ThenInclude(c => c.Category)
            .Include(p => p.PostPetCoffeeShops)
            .ThenInclude(shop => shop.Shop)
            .Include(p => p.CreatedBy).FirstOrDefaultAsync();
            if (Posts == null)
            {
                throw new ApiException(ResponseCode.PostNotExist);
            }
            //var response = new PostResponse();
            var postResponse = _mapper.Map<PostResponse>(Posts);
            

            return postResponse;

        }
    }
}
