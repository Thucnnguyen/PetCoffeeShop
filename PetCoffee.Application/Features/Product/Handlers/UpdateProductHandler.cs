using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.Product.Commands;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TmsApi.Common;

namespace PetCoffee.Application.Features.Product.Handlers
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureService _azureService;
        private readonly ICurrentAccountService _currentAccountService;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public UpdateProductHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper, ICacheService cacheService)
        {
            _unitOfWork = unitOfWork;
            _azureService = azureService;
            _currentAccountService = currentAccountService;
            _mapper = mapper;
            _cacheService = cacheService;
        }
        public async Task<ProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
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

            Assign.Partial<UpdateProductCommand, Domain.Entities.Product>(request, product);

            //upload Image
            if (request.Image != null)
            {
                product.Image = await _azureService.UpdateloadImages(request.Image);
            }

            await _unitOfWork.ProductRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<ProductResponse>(product);
            //await _cacheService.SetAsync(response.Id.ToString(), response, cancellationToken);

            return response;
        }
    }

}
