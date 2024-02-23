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
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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

            //    var query = await _unitOfWork.PostRepository.GetAsync(
            //    predicate: p => p.Id == request.Id,
            //    includes: new List<Expression<Func<PetCoffee.Domain.Entities.Post, object>>>()
            //    {
            //        p => p.Comments,
            //        p => p.Comments.,
            //        locker => locker.Location.Province,
            //        locker => locker.Location.District,
            //        locker => locker.Location.Ward,
            //        locker => locker.Store,
            //        locker => locker.Store.Location,
            //        locker => locker.Store.Location.Province,
            //        locker => locker.Store.Location.District,
            //        locker => locker.Store.Location.Ward,
            //        locker => locker.OrderTypes,
            //    },
            //    disableTracking: true
            //);


            //    var locker = query.FirstOrDefault();
            //    if (locker is null)
            //    {
            //        throw new ApiException(ResponseCode.LockerErrorNotFound);
            //    }

            //    Console.WriteLine(locker.Location.ToString());
            //    return _mapper.Map<LockerDetailResponse>(locker);
            var Posts = await _unitOfWork.PostRepository.Get(p => p.Id == request.Id && p.Status == PostStatus.Active)
            .Include(p => p.Comments)
            .ThenInclude(com => com.CreatedBy)
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
            if (Posts.Comments != null)
            {
                postResponse.Comments = Posts.Comments.ToList().Select(c => _mapper.Map<CommentForPost>(c)).ToList();
            }
            if (Posts.PostCategories != null)
            {
                postResponse.Categories = Posts.PostCategories.Select(c => _mapper.Map<CategoryForPostModel>(c.Category)).ToList();
            }

            if (Posts.PostPetCoffeeShops != null)
            {
                postResponse.PetCoffeeShops = Posts.PostPetCoffeeShops.Select(c => _mapper.Map<CoffeeshopForPostModel>(c.Shop)).ToList();
            }
            if (Posts.CreatedBy != null)
            {
                postResponse.Account = _mapper.Map<AccountForPostModel>(Posts.CreatedBy);
            }

            return postResponse;

        }
    }
}
