using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Payments.Models;
using PetCoffee.Application.Features.Payments.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Payments.Handlers;

public class GetAllTransactionHandler : IRequestHandler<GetAllTransactionQuery, PaginationResponse<Domain.Entities.Transaction, PaymentResponse>>
{
    private readonly ICurrentAccountService _currentAccountService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllTransactionHandler(ICurrentAccountService currentAccountService, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _currentAccountService = currentAccountService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PaginationResponse<Domain.Entities.Transaction, PaymentResponse>> Handle(GetAllTransactionQuery request, CancellationToken cancellationToken)
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

        if (currentAccount != null && currentAccount.IsCustomer && request.ShopId != 0)
        {
            request.CustomerId = currentAccount.Id;
        }

        var payments = _unitOfWork.TransactionRepository
           .Get(
               predicate: request.GetExpressions(),
               orderBy: request.GetOrder(),
               disableTracking: true)
                            .Include(t => t.Items)
                                .ThenInclude(ti => ti.Item)
                            .Include(t => t.Pet)
                            .Include(t => t.Reservation)
                            .ThenInclude(r => r.Area)
                            .ThenInclude(a => a.PetCoffeeShop)
                            .AsQueryable();

        return new PaginationResponse<Domain.Entities.Transaction, PaymentResponse>(
            payments,
            request.PageNumber,
            request.PageSize,
            payment => _mapper.Map<PaymentResponse>(payment));
    }
}
