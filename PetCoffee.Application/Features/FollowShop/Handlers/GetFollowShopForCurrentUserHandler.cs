
using AutoMapper;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.FollowShop.Queries;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using PetCoffee.Shared.Ultils;

namespace PetCoffee.Application.Features.FollowShop.Handlers;

public class GetFollowShopForCurrentUserHandler : IRequestHandler<GetFollowShopForCurrentUserQuery, PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;

    public GetFollowShopForCurrentUserHandler(IMapper mapper, IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
    }

    public async Task<PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>> Handle(GetFollowShopForCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var CurrentUser = await _currentAccountService.GetCurrentAccount();
        if (CurrentUser == null)
        {
            throw new ApiException(ResponseCode.AccountNotExist);
        }
        if (CurrentUser.IsVerify)
        {
            throw new ApiException(ResponseCode.AccountNotActived);
        }
        var Expression = request.GetExpressions().And(f => f.CreatedById == CurrentUser.Id);
        var follows = await _unitOfWork.FollowPetCfShopRepository.Get(Expression)
                                    .Include(f => f.Shop)
                                    .ToListAsync();

        var response = new List<PetCoffeeShopForCardResponse>();
        if (request.Longitude != 0 && request.Latitude != 0)
        {
            foreach (var f in follows)
            {
                if (f.Shop == null) continue;
                var shopResponse = _mapper.Map<PetCoffeeShopForCardResponse>(f.Shop);
                shopResponse.Distance = CalculateDistanceUltils.CalculateDistance(request.Latitude, request.Longitude, f.Shop.Latitude, f.Shop.Longitude);
                response.Add(shopResponse);
            }
            return new PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>(
                response,
                response.Count(),
                request.PageNumber,
                request.PageSize);
        }
        foreach (var f in follows)
        {
            if (f.Shop == null) continue;
            var shopResponse = _mapper.Map<PetCoffeeShopForCardResponse>(f.Shop);
            response.Add(shopResponse);
        }
        return new PaginationResponse<PetCoffeeShop, PetCoffeeShopForCardResponse>(
            response,
            response.Count(),
            request.PageNumber,
            request.PageSize);
    }
}
