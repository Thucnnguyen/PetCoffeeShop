using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Common.Enums;
using PetCoffee.Application.Common.Exceptions;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Items.Queries;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Application.Features.Moment.Queries;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Application.Service;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Items.Handlers
{
    public class GetItemCurrentAccountIdHandler : IRequestHandler<GetItemCurrentAccountIdQuery, IList<ItemResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentAccountService _currentAccountService;
        private readonly IMapper _mapper;

        public GetItemCurrentAccountIdHandler(IUnitOfWork unitOfWork, ICurrentAccountService currentAccountService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentAccountService = currentAccountService;
            _mapper = mapper;
        }
        public async Task<IList<ItemResponse>> Handle(GetItemCurrentAccountIdQuery request, CancellationToken cancellationToken)
        {
            var currentAccount = await _currentAccountService.GetRequiredCurrentAccount();
            if (currentAccount == null)
            {
                throw new ApiException(ResponseCode.AccountNotExist);
            }
            if (currentAccount.IsVerify)
            {
                throw new ApiException(ResponseCode.AccountNotActived);
            }
            //get wallet
            var wallet = await _unitOfWork.WalletRepsitory
                           .Get(m => m.Id == request.WalletId && m.AccountId == currentAccount.Id).FirstOrDefaultAsync();

            if (wallet is null)
            {
                throw new ApiException(ResponseCode.WalletNotExist);
            }
            var walletItems = await _unitOfWork.WalletItemRepository.Get(i => i.WalletId == wallet.Id)
                .Include(d => d.Item)
            .ToListAsync();




            var response = new List<ItemResponse>();
            foreach (var item in walletItems)
            {
                var itemResponse = _mapper.Map<ItemResponse>(item.Item);
                itemResponse.TotalItem = item.TotalItem;

                response.Add(itemResponse);

            }


            return response;


        }
    }
}
