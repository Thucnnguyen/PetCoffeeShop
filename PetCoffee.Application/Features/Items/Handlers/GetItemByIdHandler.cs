using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Features.Events.Queries;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Items.Queries;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Items.Handlers
{
    public class GetItemByIdHandler : IRequestHandler<GetItemByIdQuery, ItemResponse>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentAccountService _currentAccountService;
        private readonly IMapper _mapper;

        public GetItemByIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentAccountService = currentAccountService;
            _mapper = mapper;
        }

        public async Task<ItemResponse> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
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

            var item = await _unitOfWork.ItemRepository.GetByIdAsync(request.ItemId);
            if (item == null)
            {
                throw new ApiException(ResponseCode.ItemNotExist);
            }

            var response = _mapper.Map<ItemResponse>(item);
        
            return response;
        }
    }
}
