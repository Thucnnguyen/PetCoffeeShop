using AutoMapper;
using MediatR;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Areas.Commands;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Items.Commands;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Items.Handlers
{
    public class CreateItemHandler : IRequestHandler<CreateItemCommand, ItemResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentAccountService _currentAccountService;
        private readonly IMapper _mapper;

        public CreateItemHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentAccountService = currentAccountService;
            _mapper = mapper;
        }
        public async Task<ItemResponse> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            //get Current account 
            var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
            if (currentAccount == null)
            {
                throw new ApiException(ResponseCode.AccountNotExist);
            }
            if (currentAccount.IsVerify)
            {
                throw new ApiException(ResponseCode.AccountNotActived);
            }

            //check already have name item
            var item = _unitOfWork.ItemRepository.IsExisted(c => c.Name == request.Name);
            if (item)
            {
                throw new ApiException(ResponseCode.ItemNameIsExisted);
            }


            var newItem = _mapper.Map<Domain.Entities.Item>(request);
            newItem.CreatedById = currentAccount.Id;
            newItem.CreatedAt = DateTime.Now;
            await _unitOfWork.ItemRepository.AddAsync(newItem);
            await _unitOfWork.SaveChangesAsync();

            var response = _mapper.Map<ItemResponse>(newItem);

            return response;
        }
    }
}
