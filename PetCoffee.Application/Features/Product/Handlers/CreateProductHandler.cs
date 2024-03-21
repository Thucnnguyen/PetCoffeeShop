using AutoMapper;
using MediatR;
using PetCoffee.Application.Features.Product.Commands;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Product.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAzureService _azureService;
        private readonly ICurrentAccountService _currentAccountService;
        private readonly IMapper _mapper;

        public CreateProductHandler(IUnitOfWork unitOfWork, IAzureService azureService, ICurrentAccountService currentAccountService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _azureService = azureService;
            _currentAccountService = currentAccountService;
            _mapper = mapper;
        }
        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            //var currentAccount = await _currentAccountService.GetCurrentAccount();
            //if (currentAccount == null)
            //{
            //    throw new ApiException(ResponseCode.AccountNotExist);
            //}
            //if (currentAccount.IsVerify)
            //{
            //    throw new ApiException(ResponseCode.AccountNotActived);
            //}

            //var PetCoffeeShop = await _unitOfWork.PetCoffeeShopRepository.GetAsync(s => s.Id == request.PetCoffeeShopId && s.Status == ShopStatus.Active);
            //var petCoffeeShop = PetCoffeeShop.FirstOrDefault();
            //if (petCoffeeShop == null)
            //{
            //    throw new ApiException(ResponseCode.ShopNotExisted);
            //}

            //// check image  - will do later

            //var NewProduct = _mapper.Map<Domain.Entities.Product>(request);
            //var addProducted = await _unitOfWork.ProductRepository.AddAsync(NewProduct);
            //await _unitOfWork.SaveChangesAsync();

            ////var id =  addProducted.fir;
            //var response = _mapper.Map<ProductResponse>(NewProduct);

            //response.CreatedById = currentAccount.Id;

            ////
            //var petshopProduct = new PetCoffeeProduct
            //{
            //    PetCoffeeShopId = request.PetCoffeeShopId,
            //    ProductId = addProducted.Id
            //};
            //await _unitOfWork.PetCoffeeShopProductRepository.AddAsync(petshopProduct);


            //await _unitOfWork.SaveChangesAsync();
            //return response;

            return null;


        }
    }
}
