using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;

using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Features.Product.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Product.Handlers
{

    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;
        private readonly ICacheService _cacheService;

        public GetProductByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;
            _cacheService = cacheService;
        }
        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
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
            var Product = await _unitOfWork.ProductRepository
                    .Get(predicate: p => p.Id == request.Id && !p.Deleted)
                    .FirstOrDefaultAsync();


            if (Product == null)
            {
                throw new ApiException(ResponseCode.ProductNotExist);
            }

            var response = _mapper.Map<ProductResponse>(Product);

      
            return response;
        }
    }
}
