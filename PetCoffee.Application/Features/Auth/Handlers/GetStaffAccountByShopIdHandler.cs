

using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Auth.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class GetStaffAccountByShopIdHandler : IRequestHandler<GetStaffAccountByShopIdQuery, PaginationResponse<Account, AccountResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IMapper _mapper;

    public GetStaffAccountByShopIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _currentAccountService = currentAccountService;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<Account, AccountResponse>> Handle(GetStaffAccountByShopIdQuery request, CancellationToken cancellationToken)
    {
        var currentAccount = await _currentAccountService.GetCurrentAccount();
        if (currentAccount == null)
        {
            throw new ApiException(ResponseCode.AccountNotExist);
        }
        if (currentAccount.IsVerify)
        {
            throw new ApiException(ResponseCode.AccountNotActived);
        }

        if (!currentAccount.AccountShops.Any(a => a.ShopId == request.ShopId))
        {
            throw new ApiException(ResponseCode.PermissionDenied);
        };
        var staffs = await _unitOfWork.AccountRepository
                        .GetAsync(
                            predicate: a => a.IsStaff && a.FullName.Contains(request.Search) && !a.Deleted && a.AccountShops.Any(acs => acs.ShopId == request.ShopId),
                            includes: new List<System.Linq.Expressions.Expression<Func<Account, object>>>
                            {
                                a => a.AccountShops
                            });
        return new PaginationResponse<Account, AccountResponse>(
                staffs,
                request.PageNumber,
                request.PageSize,
                s => _mapper.Map<AccountResponse>(s));

    }
}
