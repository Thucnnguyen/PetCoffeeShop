using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Product.Commands;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Product.Handlers
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentAccountService _currentAccountService;
        private readonly ICacheService _cacheService;

        public DeleteProductHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentAccountService currentAccountService, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentAccountService = currentAccountService;
            _cacheService = cacheService;
        }
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
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

            if (currentAccount.IsCustomer)
            {
                throw new ApiException(ResponseCode.PermissionDenied);
            };

            var product = (await _unitOfWork.ProductRepository.GetAsync(p => p.Id == request.Id && !p.Deleted)).FirstOrDefault();

            if (product == null)
            {
                throw new ApiException(ResponseCode.ProductNotExist);
            }

            if (!currentAccount.AccountShops.Any(a => a.ShopId == product.PetCoffeeShopId))
            {
                throw new ApiException(ResponseCode.PermissionDenied);
            }

            product.DeletedAt = DateTime.UtcNow;
            product.ProductStatus = Domain.Enums.ProductStatus.Inactive;
            await _unitOfWork.ProductRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

}
